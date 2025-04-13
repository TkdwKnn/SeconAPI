import {axiosInstance} from "../instance.ts";
import {ILoginRequest, ILoginResponse} from "./types.ts";
import endpoints from "../endpoints.ts";
import {AxiosPromise} from "axios";

export const login = (params: ILoginRequest): AxiosPromise<ILoginResponse> =>
    axiosInstance.post(endpoints.auth.login, params);

export const logout = () => axiosInstance.post(endpoints.auth.logout);
