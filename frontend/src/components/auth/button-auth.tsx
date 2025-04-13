import {FC} from "react";

interface IButtonAuth {
    title: string;
}

const ButtonAuth: FC<IButtonAuth> = ({title}) => {
    return (
        <button className={"w-[420px] bg-transparent cursor-pointer text-white text-[24px] roboto border-[#BCA95A] border-2 rounded-[12px] h-[66px]"}>{title}</button>
    );
};

export default ButtonAuth;