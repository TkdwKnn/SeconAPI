import {Avatar, Button, Popover} from "antd";
import {LockOutlined, UserOutlined} from "@ant-design/icons";

const BlockHeader = () => {
    return (
        <div className={"w-full h-[64px] bg-white"}>
            <div className={"w-[1100px] h-full flex justify-between items-center m-auto"}>
                <div>
                    <div>Zalupa.рф</div>
                </div>
                <div>
                    <Popover content={<Button icon={<LockOutlined />} className={"w-full text-start"} color={"danger"} variant="solid">Выйти</Button>} className={"flex gap-2 items-center cursor-pointer"} title="Меню" trigger="click">
                        <div>USER_SAMPLE</div>
                        <Avatar size={48} icon={<UserOutlined/>}/>
                    </Popover>

                </div>
            </div>
        </div>
    );
};

export default BlockHeader;