import {FC} from "react";
import ButtonDefault from "./button-default.tsx";
import styles from "./element-table-item.module.css";
import {useAppDispatch} from "../../hooks.ts";
import {deletePhoto, deleteReestr} from "../../redux/main.slice.ts";

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
            <div className={"cursor-default"}>{file.name}</div>
            <div className={styles.deleteButton}>
                <ButtonDefault onClick={onDeleteClickHandler} title={"Удалить"} className={"text-[#F12727] "}/>
            </div>
        </div>
    );
};

export default ElementTableItem;