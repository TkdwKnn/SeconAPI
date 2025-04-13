import BlockTable from "../components/main/block-table.tsx";
import {useAppDispatch, useAppSelector} from "../hooks.ts";
import ButtonScan from "../components/main/button-scan.tsx";
import PageMainBase from "./page-main-base.tsx";
import {uploadFilesAC} from "../redux/miscACs.ts";
import {useNavigate} from "react-router";
import BlockEmptyNotif from "../components/main/block-empty-notif.tsx";

const PageUpload = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const reestr = useAppSelector(state => state.main.reestrFileList);
    const photos = useAppSelector(state => state.main.photosFileList);

    const handleScanClick = async () => {
        await dispatch(uploadFilesAC([...reestr, ...photos]));
        navigate("/result");
    }

    return (
        <PageMainBase>
            <div className={"w-[1150px] flex flex-col items-center m-auto"}>
                {reestr.length === 0 && photos.length === 0
                    && <div className={"mt-[70px]"}>
                        <BlockEmptyNotif title={"Начните загружать файлы и они появятся здесь"}/>
                </div>
                }
                {reestr.length !== 0 && <BlockTable title={"Excel"} items={reestr}/>}
                {photos.length !== 0 && <BlockTable title={"Фото"} items={photos}/>}
            </div>
            <ButtonScan onClick={handleScanClick} title={"Сканировать"} disabled={reestr.length == 0 || photos.length == 0}/>
        </PageMainBase>
    );
};

export default PageUpload;