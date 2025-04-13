import BaseBlockMain from "../common/base-block-main.tsx";
import {List} from "antd";
import ButtonUpload from "./button-upload.tsx";
import ElementReestrItem from "./element-reestr-item.tsx";
import {useAppDispatch, useAppSelector} from "../../hooks.ts";
import {setReestrFileList} from "../../redux/main.slice.ts";

const BlockReestr = () => {
    const dispatch = useAppDispatch();
    const reestr = useAppSelector(state => state.main.reestrFileList);
    //const [reestr, setReestr] = useState<File[]>([]);

    const handleOnChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            dispatch(setReestrFileList(e.target.files));
        }
    }

    return (
        <BaseBlockMain>
            <div className={"flex flex-col gap-2 w-full"}>
                <div className={"text-[20px]"}>Загрузить файл реестра</div>
                <div className={"flex flex-col gap-2 w-full"}>
                    <ButtonUpload type={"full"} onChange={handleOnChange}/>
                    {reestr.length !== 0 && <List
                        size="large"
                        header={"Загруженные файлы"}
                        bordered
                        dataSource={reestr}
                        renderItem={(item) => <List.Item>
                            <ElementReestrItem file={item}/>
                        </List.Item>}
                    />
                    }

                </div>

            </div>
        </BaseBlockMain>
    );
};

export default BlockReestr;