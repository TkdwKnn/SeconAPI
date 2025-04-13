import {FC, useState} from "react";
import {CloseOutlined, EyeOutlined} from "@ant-design/icons";
import {Modal} from "antd";
import {useAppDispatch} from "../../hooks.ts";
import {deletePhoto} from "../../redux/main.slice.ts";

interface IElementImageItem {
    file: File;
}

const ElementImageItem: FC<IElementImageItem> = ({file}) => {
    const dispatch = useAppDispatch();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const image = URL.createObjectURL(file);

    const handleOk = () => {
        setIsModalOpen(false);
    };

    const handleCancel = () => {
        setIsModalOpen(false);
    };

    const handleDeleteClick = () => {
        dispatch(deletePhoto(file))
    }

    return (
        <div>
            <Modal title="Просмотр" footer={[]} open={isModalOpen} onOk={handleOk} onCancel={handleCancel}>
                <img src={image} alt=""/>
            </Modal>

            <div className={"w-[100px] h-[100px]"}>
                <div className={"relative"}>
                    <img className={"w-[100px] h-[100px] absolute rounded-md"} src={image} alt=""/>
                    <div className={"w-[100px] h-[100px] text-transparent bg-transparent " +
                        "absolute hover:text-white hover:bg-[#00000066] rounded-md transition-all transition-duration-200"}>
                        <button className={"w-[50px] h-[100px] cursor-pointer"} onClick={() => {
                            setIsModalOpen(true)
                        }}>
                            <EyeOutlined className={"text-[30px] hover:text-[40px] transition-all transition-duration-200"}/>
                        </button>
                        <button className={"w-[50px] h-[100px] cursor-pointer"} onClick={handleDeleteClick}>
                            <CloseOutlined className={"text-[30px] text-red-600 hover:text-[40px] transition-all transition-duration-200"}/>
                        </button>
                    </div>

                </div>
            </div>
        </div>

    );
};

export default ElementImageItem;