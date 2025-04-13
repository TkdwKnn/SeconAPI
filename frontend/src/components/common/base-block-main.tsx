import {FC, ReactElement} from 'react';

interface IBaseBlockMain {
    children: ReactElement | ReactElement[];
}

const BaseBlockMain: FC<IBaseBlockMain> = ({children}) => {
    return (
        <div className={"w-full bg-white rounded-lg flex justify-start items-start box-border p-3"}>
            {children}
        </div>
    );
};

export default BaseBlockMain;