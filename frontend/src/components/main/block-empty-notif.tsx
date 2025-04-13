import {FC} from 'react';

interface IProps {
    title: string;
}

const BlockEmptyNotif: FC<IProps> = ({title}) => {
    return (
        <div className={"text-[24px] text-[#6540C5]"}>
            {title}
        </div>
    );
};

export default BlockEmptyNotif;