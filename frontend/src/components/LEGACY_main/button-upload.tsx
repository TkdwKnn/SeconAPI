import {PlusOutlined} from "@ant-design/icons";
import {FC} from "react";

interface IButtonUpload {
    size?: number;
    onChange?: (value: any) => void;
    onClick?: (value: any) => void;
    onSubmit?: (value: any) => void;
    type?: "default" | "full";
}

const ButtonUpload: FC<IButtonUpload> = ({onChange, onClick, onSubmit, type = "default"}) => {

    let wrapperStyle: string = "";
    let divStyle: string = "";
    let inputStyle: string = "";

    if (type === "default") {
        wrapperStyle = "w-[100px] h-[100px] relative";
        divStyle = "text-[#9197f2] absolute flex flex-col border-2 justify-center rounded-md items-center cursor-pointer w-[100px] h-[100px] transition-all duration-200 hover:text-black";
        inputStyle = "absolute w-[100px] h-[100px] cursor-pointer text-transparent";
    } else {
        wrapperStyle = "w-full h-[100px] relative";
        divStyle = "text-[#9197f2] absolute flex flex-col border-2 justify-center rounded-md items-center cursor-pointer w-full h-[100px] transition-all duration-200 hover:text-black";
        inputStyle = "absolute w-full h-[100px] cursor-pointer text-transparent";
    }

    return (
        <div className={wrapperStyle}>
            <div
                className={divStyle}>
                <PlusOutlined className={"text-[32px]"} size={64}/>
                <div className={"text-sm"}>Загрузить</div>
                <input multiple type={"file"}
                       className={inputStyle}
                       onChange={onChange}
                       onClick={onClick}
                       onSubmit={onSubmit}
                >

                </input>
            </div>
            <div>

            </div>

        </div>

    );
};

export default ButtonUpload;