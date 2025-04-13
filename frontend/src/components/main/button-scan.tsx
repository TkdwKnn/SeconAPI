import ButtonDefault, {IButton} from "./button-default.tsx";
import {FC} from "react";

const ButtonScan: FC<IButton> = ({disabled, onClick, title}) => {

    return (
        <div className={"fixed right-[64px] bottom-[9px]"}>
            <ButtonDefault onClick={onClick} disabled={disabled} title={title}
                           className={(disabled ? "bg-[#878787]" : "bg-[#6540C5]") + " w-[218px] h-[54px] text-white justify-center rounded-[4px]"}/>
        </div>
    );
};

export default ButtonScan;