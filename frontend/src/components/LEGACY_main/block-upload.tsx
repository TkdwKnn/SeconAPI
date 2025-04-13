import ElementUploader from "./element-uploader.tsx";
import BaseBlockMain from "../common/base-block-main.tsx";


const BlockUpload = () => {
    return (
        <BaseBlockMain>
            <div className={"flex justify-start items-start box-border w-full gap-3"}>
                <ElementUploader title={"Загрузить фотоотчеты счетчиков"}/>
            </div>
        </BaseBlockMain>
    );
};

export default BlockUpload;