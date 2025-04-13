import {useEffect, useState} from 'react';
import ElementHistoryItem from "../components/history/element-history-item.tsx";

import styles from "./page-history.module.css"
import ButtonDefault from "../components/main/button-default.tsx";
import PageMainBase from "./page-main-base.tsx";
import {useAppDispatch, useAppSelector} from "../hooks.ts";
import {deleteHistoryItemAC, getHistoryAC} from "../redux/miscACs.ts";
import {BASE_URL} from "../api/endpoints.ts";
import BlockEmptyNotif from "../components/main/block-empty-notif.tsx";

const PageHistory = () => {
    const [selectedItems, setSelectedItems] = useState<number[]>([]);
    const dispatch = useAppDispatch();
    const history = useAppSelector(state => state.misc.myHistoryData.history);

    const historyEls = history ? history.map((item) => (
        <ElementHistoryItem state={selectedItems} setSelectHandler={setSelectedItems} num={item.id} title={item.resultArchiveFileName + ".zip"} selected={selectedItems.includes(item.id)} key={item.id} />
    )) : [];

    useEffect(() => {
        dispatch(getHistoryAC());
    }, [dispatch]);

    const handleOnDownloadClick = () => {
        console.log(selectedItems);
        window.open(`${BASE_URL}/Documents/task/download/${selectedItems}`);
        setSelectedItems([]);
    }

    const handleDeleteClick = async () => {
        await dispatch(deleteHistoryItemAC(selectedItems));
        setSelectedItems([]);
    }

    return (
        <PageMainBase>
            <div className={"w-[1100px] m-auto"}>


            {history && history.length !== 0
                ? <div className={"mt-[20px] flex flex-col gap-4  "}>
                    <div className={"text-[24px] roboto font-[700]"}>История</div>
                    <div className={"flex-wrap flex gap-6 items-start"}>
                        {historyEls}
                    </div>

                </div>
                : <div className={"mt-[70px] flex w-full justify-center"}>
                    <BlockEmptyNotif title={"Начните работу и ваша история появится здесь"}/>
                </div>}
            </div>
            <div>
                {selectedItems.length > 0
                    && <div className={"fixed left-[18px] bottom-[32px] " + styles.manageDown}>
                        <ButtonDefault onClick={handleOnDownloadClick} title={"Скачать"}
                                       className={"bg-[#6540C5] text-white w-[169px] h-[54px] flex items-center justify-center rounded-[4px]"}/>
                        <ButtonDefault onClick={handleDeleteClick} title={"Удалить"} className={"text-[#F12727]"}/>
                    </div>
                }
            </div>

        </PageMainBase>
    );
};

export default PageHistory;