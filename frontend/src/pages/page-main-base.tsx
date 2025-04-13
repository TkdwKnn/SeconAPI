import {FC, ReactElement} from 'react';
import BlockHeader from "../components/main/block-header.tsx";

interface IPageBase {
    children?: ReactElement | ReactElement[];
}

const PageMainBase: FC<IPageBase> = ({children}) => {
    if (localStorage.getItem("token") == "" || !localStorage.getItem("token")) window.location.href = "/auth/login"

    return (
        <div className={"w-screen"}>
            <BlockHeader/>
            {children}
        </div>
    );
};

export default PageMainBase;