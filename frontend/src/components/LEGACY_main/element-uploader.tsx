import {FC} from "react";
import ButtonUpload from "./button-upload.tsx";
import ElementImageItem from "./element-image-item.tsx";
import {useAppDispatch, useAppSelector} from "../../hooks.ts";
import {setPhotosFileList} from "../../redux/main.slice.ts";

interface IUploader {
    title: string;
}

const ElementUploader: FC<IUploader> = ({title}) => {
    const dispatch = useAppDispatch();
    const uploadedFiles = useAppSelector(state => state.main.photosFileList);

    const imgEls = uploadedFiles.map((file, index) => <ElementImageItem key={index} file={file}/>)

    const handleFilesUploadChange = (event: any) => {
        dispatch(setPhotosFileList(event.target.files));
    }

    return (
        <div className={"w-full"}>
            <div className={"text-[20px] pb-2"}>{title}</div>
            <div className={"flex gap-2 flex-wrap min-h-[100px]"}>
                {imgEls}
                <ButtonUpload onChange={handleFilesUploadChange}/>
            </div>
        </div>
    );
};

export default ElementUploader;