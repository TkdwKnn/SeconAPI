import ButtonDefault from "./button-default.tsx";
import icon1 from "../../assets/icons/file-earmark-arrow-up.svg";
import icon2 from "../../assets/icons/file-earmark-arrow-up2.svg";
import {useAppDispatch} from "../../hooks.ts";
import {setPhotosFileList, setReestrFileList} from "../../redux/main.slice.ts";
import {useNavigate} from "react-router";
import {logoutUser} from "../../redux/authACs.ts";
import {Popover} from "antd";

const BlockHeader = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const handleExcelUpload = (e) => {
        dispatch(setReestrFileList(e.target.files));
        navigate("/upload");
    }

    const handlePhotosUpload = (e) => {
        dispatch(setPhotosFileList(e.target.files));
        navigate("/upload");
    }

    const handleLogoutClick = async () => {
        await dispatch(logoutUser());
        navigate("/auth/login");
    }

    const handleHistoryClick = () => {
        navigate("/history");
    }

    const handleUploadClick = () => {
        navigate("/upload");
    }

    return (
        <div className="w-full h-[80px] border-1">
            <div className={"w-[1100px] h-[80px] flex justify-between items-center m-auto"}>
                <div className={"flex gap-12"}>
                    <div className={"roboto flex flex-col text-[24px]"}>
                        <span>СканЭнерго</span>
                    </div>
                    <div className={"flex gap-6"}>
                        <ButtonDefault icon={icon1} onFileChange={handleExcelUpload} type={"file"} title={"Загрузить Excel"}
                                       className={"text-[#5ACC00]"}/>
                        <ButtonDefault icon={icon2} onFileChange={handlePhotosUpload} type={"file"} title={"Загрузить Фото"}
                                       className={"text-[#6540C5]"}/>
                    </div>
                </div>
                <div>
                    <Popover content={<div className={"flex flex-col gap-4"}>
                        <ButtonDefault onClick={handleHistoryClick} title={"История"}
                                       className={"text-[#6540C5]"}/>
                        <ButtonDefault onClick={handleUploadClick} title={"Загрузить документы"}
                                       className={"text-[#6540C5]"}/>
                        <ButtonDefault onClick={handleLogoutClick} title={"Выйти"}
                                       className={"text-[#F12727]"}/>
                    </div>} className={"text-[18px]"} title={<div className={"text-[18px]"}>Меню</div>} trigger="click">
                        <button className={"text-[18px] roboto text-[#6540C5] cursor-pointer"}>Профиль</button>
                    </Popover>
                </div>
            </div>
        </div>
    );
};

export default BlockHeader;