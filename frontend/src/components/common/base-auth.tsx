import {FC, ReactNode} from "react";

interface IBaseAuth {
    title: string;
    children: ReactNode;
}

const BaseAuth: FC<IBaseAuth> = ({children, title}) => {
    return (
        <div className="w-[430px] mx-auto bg-white p-4 rounded-md">
            <div className={"text-black text-[30px] flex w-full justify-start roboto"}>{title}</div>
            {children}
        </div>
    );
};

export default BaseAuth;