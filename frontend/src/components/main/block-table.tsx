import {FC, useState} from 'react';
import ButtonDefault from "./button-default.tsx";
import ElementTableItem from "./element-table-item.tsx";
import {useAppDispatch} from "../../hooks.ts";
import {deleteAll} from "../../redux/main.slice.ts";

interface ITable {
    title: string;
    items?: File[];
}

const BlockTable: FC<ITable> = ({items, title}) => {
    //const [selectedItems, setSelectedItems] = useState<number[]>([]);
    const dispatch = useAppDispatch();
    const [isAllSelected, setIsAllSelected] = useState(false);
    const tableItems = items?.map((item, index) => {

        return <ElementTableItem key={index} index={index} selected={isAllSelected} file={item}/>
    })

    const handleSelectAll = () => {
        setIsAllSelected(true);
    }

    const handleDeselectAll = () => {
        setIsAllSelected(false);
    }

    const handleDeleteAll = () => {
        if (items) {
            dispatch(deleteAll(items));
        }
        setIsAllSelected(false);
    }

    return (
        <div className={"w-full mt-[30px]"}>
            <div className={"w-[1100px] m-auto flex justify-between"}>
                <div className={"text-[24px] roboto font-[700]"}>{title}</div>
                {isAllSelected
                    ? <div className={"flex gap-[18px]"}>
                        <ButtonDefault onClick={handleDeselectAll} title={"Отменить выделение"} className={"text-[#6540C5]"}/>
                        <ButtonDefault onClick={handleDeleteAll} title={"Удалить"} className={"text-[#F12727]"}/>
                    </div>
                    : <ButtonDefault onClick={handleSelectAll} title={"Выделить всё"} className={"text-[#C126D9]"}/>
                }
            </div>
            <div className={"border-t"}>
                <div className={""}>
                    {tableItems}
                </div>

            </div>
        </div>
    );
};

export default BlockTable;