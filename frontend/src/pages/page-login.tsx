import BaseAuth from "../components/common/base-auth.tsx";
import {Button} from "antd";
import {useNavigate} from "react-router";
import {FC, useState} from "react";
import {IRouter} from "../routers/router-auth.tsx";
import InputAuth from "../components/auth/input-auth.tsx";
import {useAppDispatch} from "../hooks.ts";
import {loginUser} from "../redux/authACs.ts";



const PageLogin: FC<IRouter> = ({sourceDir}) => {
    const dispatch = useAppDispatch();

    if (localStorage.getItem("token") !== "" || localStorage.getItem("token")) window.location.href = "/history"

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleUsernameChange = (e) => {
        setUsername(e.target.value);
    }

    const handlePasswordChange = (e) => {
        setPassword(e.target.value);
    }

    const handleLoginClick = () => {
        dispatch(loginUser({username, password}));
    }

    return (
        <div className="bg-slate-50 w-screen h-screen flex flex-col justify-center">
            <BaseAuth title={"Вход"}>
                <div className="flex flex-col gap-4 py-4">
                    <InputAuth value={username} onChange={handleUsernameChange} title={"Имя пользователя"}
                               placeholder={"Введите имя пользователя"}/>
                    <InputAuth value={password} onChange={handlePasswordChange} type={"password"} title={"Пароль"}
                               placeholder={"Введите пароль"}/>
                </div>
                <div className={"flex flex-col items-start gap-6 pt-2"}>
                    <Button style={{ background: "#6540C5"}} className={"w-full"} size={"large"} type={"primary"} onClick={handleLoginClick}>Войти</Button>
                </div>
            </BaseAuth>
        </div>
    );
};

export default PageLogin;