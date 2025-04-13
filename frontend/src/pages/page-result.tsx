import PageMainBase from "./page-main-base.tsx";
import {useAppSelector} from "../hooks.ts";
import {BASE_URL} from "../api/endpoints.ts";
import downloadIcon from "../assets/icons/Download - Iconly Pro.svg";
import styles from "./page-result.module.css"

const PageResult = () => {
    const taskId = useAppSelector(state => state.misc.documentProcessData.taskId);

    return (
        <PageMainBase>
            <div className={"flex mt-[100px] justify-center items-center"}>
                <a className={"flex flex-col justify-center items-center hover:"} href={`${BASE_URL}/Documents/task/${taskId}`}>
                    <img className={"w-[150px]"} src={downloadIcon} alt=""/>
                    <div className={"text-[36px] flex justify-center text-[#6540C5]"}>Скачать архив</div>
                </a>

            </div>
        </PageMainBase>
    );
};

export default PageResult;