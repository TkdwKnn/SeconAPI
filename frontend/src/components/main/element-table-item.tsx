import {FC} from "react";
import ButtonDefault from "./button-default.tsx";
import styles from "./element-table-item.module.css";
import {useAppDispatch} from "../../hooks.ts";
import {deletePhoto, deleteReestr} from "../../redux/main.slice.ts";
import {DeleteOutlined, FileOutlined} from "@ant-design/icons";

interface ITableItem {
    file: File;
    index?: number;
    selected?: boolean;
}

const ElementTableItem: FC<ITableItem> = ({file, index, selected = false}) => {
    const dispatch = useAppDispatch();

    const onDeleteClickHandler = () => {
        dispatch(deleteReestr(file));
        dispatch(deletePhoto(file));
    }

    const mainStyle = (selected ? ((index % 2 == 0) ? "bg-[#F1EDFC]" : "bg-[#E5DDFB]") : ((index % 2 == 0) ? "bg-white" : "bg-[#F2F2F2] ")) + " w-full h-[44px] px-[25px] flex justify-between items-center hover:bg-[#E5DDFB] "
        + styles.item

    return (
        <div className={mainStyle}>
            <div className={"flex justify-center items-center gap-1"}>
                <FileOutlined className={"text-[18px]"}/>
                <div className={"cursor-default"}>{file.name}</div>
            </div>

            <div className={styles.deleteButton}>
                <ButtonDefault antDIcon={<DeleteOutlined className={"text-[18px]"}/>} onClick={onDeleteClickHandler} title={"Удалить"} className={"text-[#F12727] transition-all duration-100 hover:text-red-400 gap-1"}/>
            </div>
        </div>
    );
};

export default ElementTableItem;