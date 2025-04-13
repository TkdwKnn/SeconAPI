import {Button} from "antd";
import BaseAuth from "../components/common/base-auth.tsx";
import {useNavigate} from "react-router";
import {FC} from "react";
import {IRouter} from "../routers/router-auth.tsx";
import InputAuth from "../components/auth/input-auth.tsx";

const PageRegister: FC<IRouter> = ({sourceDir}) => {
    const navigate = useNavigate();

    const toLoginHandleClick = () => {
        navigate(`${sourceDir}/login`);
    }

    return (
        <div className="bg-slate-50 w-screen h-screen flex flex-col justify-center">
            <BaseAuth title={"Регистрация"}>
                <div className="flex flex-col gap-4 py-4">
                    <InputAuth title={"Имя пользователя"} placeholder={"Введите имя пользователя или email"}/>
                    <InputAuth title={"Email"} placeholder={"Введите email"}/>
                    <InputAuth title={"Пароль"} placeholder={"Введите пароль"}/>
                    <InputAuth title={"Повторите пароль"} placeholder={"Введите пароль еще раз"}/>
                </div>
                <div className={"flex flex-col items-start gap-6 pt-2"}>
                    <Button className={"w-full"} size={"large"} type={"primary"}>Зарегистрироваться</Button>
                    <div>
                        <div className="text-black">
                            <span>Есть аккаунт? </span>
                            <span onClick={toLoginHandleClick} className={"font-semibold hover:underline cursor-pointer"}>Войти</span>
                        </div>
                    </div>
                </div>
            </BaseAuth>
        </div>
    );
};

export default PageRegister;