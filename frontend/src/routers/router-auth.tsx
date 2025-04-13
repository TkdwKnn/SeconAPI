import {Route, Routes} from "react-router";
import {FC} from "react";
import PageLogin from "../pages/page-login.tsx";

export interface IRouter {
    sourceDir: string;
}

const RouterAuth: FC<IRouter> = ({sourceDir}) => {
    return (
        <div>
            <Routes>
                <Route path={`${sourceDir}/login`} element={<PageLogin sourceDir={sourceDir}/>} />
                {/*<Route path={`${sourceDir}/register`} element={<PageRegister sourceDir={sourceDir}/>} />*/}
            </Routes>
        </div>
    );
};

export default RouterAuth;