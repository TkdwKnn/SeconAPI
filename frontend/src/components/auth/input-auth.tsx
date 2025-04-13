import {FC} from 'react';
import {Input} from "antd";

interface IInputAuth {
    title: string;
    placeholder: string;
    type?: string;
    value?: string;
    onChange?: (e: any) => void;
}

const InputAuth: FC<IInputAuth> = ({placeholder, title, type, value, onChange}) => {
    return (
        <div className={"flex flex-col gap-1.5"}>
            <div>{title}</div>
            <Input onChange={onChange} value={value} type={type} style={{ background: "#EBE4FC", borderColor: "transparent"}} className={"bg-[#EBE4FC]"} size={"large"} placeholder={placeholder} />
        </div>
    );
};

export default InputAuth;