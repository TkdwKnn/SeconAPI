import {CloseOutlined, FileExcelOutlined} from "@ant-design/icons";
import {FC} from "react";
import {Button} from "antd";
import {useAppDispatch} from "../../hooks.ts";
import {deleteReestr} from "../../redux/main.slice.ts";

interface IElementReestrItem {
    file: File;
}

const ElementReestrItem: FC<IElementReestrItem> = ({file}) => {
    const dispatch = useAppDispatch();

    const handleDeleteClick = () => {
        dispatch(deleteReestr(file));
    }

    return (
        <div className={"flex items-center justify-between w-full"}>
            <div className={"flex gap-2 items-center"}>
                <FileExcelOutlined className={"text-[25px] leading-[14px]"}/>
                <div>{file.name}</div>
            </div>
            <div>
                <Button onClick={handleDeleteClick} className={"flex items-center"} icon={<CloseOutlined />}/>
            </div>
        </div>
    );
};

export default ElementReestrItem;