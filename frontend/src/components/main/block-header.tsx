import ButtonDefault from "./button-default.tsx";
import icon1 from "../../assets/icons/file-earmark-arrow-up.svg";
import icon2 from "../../assets/icons/file-earmark-arrow-up2.svg";
import {useAppDispatch} from "../../hooks.ts";
import {setPhotosFileList, setReestrFileList} from "../../redux/main.slice.ts";
import {useNavigate} from "react-router";
import {logoutUser} from "../../redux/authACs.ts";
import {Popover} from "antd";
import {HistoryOutlined, LogoutOutlined, UploadOutlined, UserOutlined} from "@ant-design/icons";

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
                <div className={"flex gap-12 items-center"}>
                    <div className={"roboto flex flex-col text-[24px]"}>
                        <span>СканЭнерго</span>
                    </div>
                    <div className={"flex gap-6"}>
                        <ButtonDefault icon={icon1} onFileChange={handleExcelUpload} type={"file"} title={"Загрузить Excel"}
                                       className={"text-[#5ACC00] rounded-[4px] p-2 gap-2 hover:bg-slate-100"}/>
                        <ButtonDefault icon={icon2} onFileChange={handlePhotosUpload} type={"file"} title={"Загрузить Фото"}
                                       className={"text-[#6540C5] rounded-[4px] p-2 gap-2 hover:bg-slate-100"}/>
                    </div>
                </div>
                <div>
                    <Popover content={<div className={"flex flex-col"}>
                        <ButtonDefault antDIcon={<HistoryOutlined className={"text-[20px]"} />} onClick={handleHistoryClick} title={"История"}
                                       className={"text-[#6540C5] rounded-[4px] p-2 gap-2 hover:bg-slate-100"}/>
                        <ButtonDefault antDIcon={<UploadOutlined className={"text-[20px]"}/>} onClick={handleUploadClick} title={"Загрузить документы"}
                                       className={"text-[#6540C5] rounded-[4px] p-2 gap-2 hover:bg-slate-100"}/>
                        <ButtonDefault antDIcon={<LogoutOutlined className={"text-[20px]"}/>} onClick={handleLogoutClick} title={"Выйти"}
                                       className={"text-[#F12727] rounded-[4px] p-2 gap-2 hover:bg-slate-100"}/>
                    </div>} className={"text-[18px]"} title={<div className={"text-[18px]"}>Меню</div>} trigger="click">
                        <div className={"cursor-pointer flex gap-2 rounded-[4px] p-3 hover:bg-slate-100"}>
                            <UserOutlined style={{color: "#6540C5", fontSize: 40}} />
                            <button className={"text-[18px] roboto text-[#6540C5] cursor-pointer"}>Профиль</button>
                        </div>

                    </Popover>
                </div>
            </div>
        </div>
    );
};

export default BlockHeader;