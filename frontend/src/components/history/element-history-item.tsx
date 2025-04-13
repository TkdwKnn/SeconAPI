import styles from "./element-history-item.module.css";
import {FC} from "react";

interface IHistoryItem {
    selected?: boolean;
    title: string;
    setSelectHandler: (value: number[]) => void;
    num: number;
    state: number[]
}

const ElementHistoryItem: FC<IHistoryItem> = ({selected, title, setSelectHandler, num, state}) => {
    const handleOnClick = () => {
        if (selected) {
            setSelectHandler([...state.filter(item => item !== num)]);
        } else setSelectHandler([...state, num]);
    }

    return (
        <div onClick={handleOnClick} className={styles.item + " " + (selected ? styles.selected : "")}>
            <div className={"text-black cursor-pointer select-none text-[18px] " + (selected ? styles.selectedItemText : "")}>{title}</div>
        </div>
    );
};

export default ElementHistoryItem;