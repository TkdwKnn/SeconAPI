import {FC} from "react";
import {IRouter} from "./router-auth.tsx";
import {Route, Routes} from "react-router";
import PageUpload from "../pages/page-upload.tsx";
import Redirector from "../components/common/redirector.tsx";
import PageHistory from "../pages/page-history.tsx";
import PageResult from "../pages/page-result.tsx";


const RouterMain: FC<IRouter> = ({sourceDir}) => {

    return (
        <div>
            <Routes>
                <Route path={`${sourceDir}`} element={<Redirector to={`/upload`}/>} />
                <Route path={`${sourceDir}/upload`} element={<PageUpload/>} />
                <Route path={`${sourceDir}/history`} element={<PageHistory/>} />
                <Route path={`${sourceDir}/result`} element={<PageResult/>} />
            </Routes>
        </div>
    );
};

export default RouterMain;