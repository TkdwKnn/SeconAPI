from flask import Flask, request, jsonify
import os
import torch
import tempfile
from PIL import Image
from ultralytics import YOLO
from transformers import TrOCRProcessor, VisionEncoderDecoderModel
from transformers.utils import logging
from classification import model, transform

app = Flask(__name__)

device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
print(f"Device: {device}")

model_yolo_old = YOLO("runs/segment/train3/weights/best.pt")
model_yolo_new = YOLO("runs/segment/train5/weights/best.pt")

model_trocr = VisionEncoderDecoderModel.from_pretrained("microsoft/trocr-base-printed").to(device)
processor = TrOCRProcessor.from_pretrained("microsoft/trocr-base-printed", use_fast=True)

model.load_state_dict(torch.load("meter_classifier.pth", map_location=device))
model.eval()

logging.set_verbosity_error()

def classify_image(img):
    img_t = transform(img).unsqueeze(0).to(device)
    with torch.no_grad():
        output = model(img_t)
        predicted = torch.argmax(output, 1).item()
    return "Старый" if predicted == 1 else "Новый"


@app.route('/batch-process', methods=['POST'])
def batch_process():
    if 'images' not in request.files:
        return jsonify({"error": "No image files provided"}), 400

    files = request.files.getlist('images')
    if not files or files[0].filename == '':
        return jsonify({"error": "No images selected"}), 400

    results = []

    with tempfile.TemporaryDirectory() as temp_dir:
        for file in files:
            temp_file_path = os.path.join(temp_dir, file.filename)
            file.save(temp_file_path)

            image = Image.open(temp_file_path).convert("RGB")

            image_class = classify_image(image)

            yolo_model = model_yolo_old if image_class == "Старый" else model_yolo_new

            yolo_results = yolo_model(temp_file_path)
            boxes = yolo_results[0].boxes.xyxy

            if boxes is None or len(boxes) == 0:
                results.append("")
                continue

            # Обрезаем по первой найденной области
            x1, y1, x2, y2 = map(int, boxes[0])
            cropped = image.crop((x1, y1, x2, y2))

            pixel_values = processor(images=cropped, return_tensors="pt").pixel_values.to(device)
            with torch.no_grad():
                generated_ids = model_trocr.generate(pixel_values)
            recognized_text = processor.batch_decode(generated_ids, skip_special_tokens=True)[0]

            recognized_text = recognized_text.replace(" ", "").replace("\n", " ")

            results.append(recognized_text)

    return jsonify(results)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001)
