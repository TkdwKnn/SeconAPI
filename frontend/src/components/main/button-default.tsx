import {FC, ReactNode} from "react";
import styles from "./button-default.module.css"

export interface IButton {
    title: string,
    icon?: string,
    antDIcon?: ReactNode,
    className?: string,
    onClick?: () => void,
    type?: "default" | "file"
    onFileChange?: (e: React.ChangeEvent<HTMLInputElement>) => void
    disabled?: boolean
}

const ButtonDefault: FC<IButton> = ({icon, title, className, onClick, type = "default", onFileChange, disabled, antDIcon}) => {
    return (
        <button disabled={disabled} className={"cursor-pointer relative " + (type === "file" ? "rounded-[4px] p-1 hover:bg-slate-100" : "")} onClick={onClick}>
            <div className={"cursor-pointer flex items-center gap-0 " + className}>
                {icon && <img className={"cursor-pointer"} src={icon} alt=""/>}
                {antDIcon && antDIcon}
                <div className={"text-[18px] roboto cursor-pointer"}>{title}</div>
            </div>
            {type === "file"
                && <input multiple type={"file"}
                          className={"absolute w-full top-0 left-0 h-full cursor-pointer text-transparent"}
                          onChange={onFileChange}
                          placeholder={""}
            />}



        </button>
    );
};

export default ButtonDefault;