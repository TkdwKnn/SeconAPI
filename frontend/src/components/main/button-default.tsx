import {FC} from "react";

export interface IButton {
    title: string,
    icon?: string,
    className?: string,
    onClick?: () => void,
    type?: "default" | "file"
    onFileChange?: (e: React.ChangeEvent<HTMLInputElement>) => void
    disabled?: boolean
}

const ButtonDefault: FC<IButton> = ({icon, title, className, onClick, type = "default", onFileChange, disabled}) => {
    return (
        <button disabled={disabled} className={"cursor-pointer relative "} onClick={onClick}>
            <div className={"flex items-center gap-2 " + className}>
                {icon && <img src={icon} alt=""/>}
                <div className={"text-[18px] roboto"}>{title}</div>
            </div>
            {type === "file"
                && <input multiple type={"file"}
                          className={"absolute w-full top-0 left-0 h-full cursor-pointer text-transparent"}
                          onChange={onFileChange}
            />}



        </button>
    );
};

export default ButtonDefault;