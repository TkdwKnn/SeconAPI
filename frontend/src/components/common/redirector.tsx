import {useNavigate} from "react-router";
import {FC, useEffect} from "react";

interface IRedirector {
    to: string;
}

const Redirector: FC<IRedirector> = ({to}) => {
    const navigate = useNavigate();

    useEffect(() => {
        navigate(to);
    }, [])

    return (
        <div>

        </div>
    );
};

export default Redirector;