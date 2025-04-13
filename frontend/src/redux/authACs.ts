import {ILoginRequest} from "../api/auth/types.ts";
import {Dispatch} from "@reduxjs/toolkit";
import {loginFailure, loginStart, loginSuccess, resetAuth} from "./auth.slice.ts";
import api from "../api"

export const loginUser = (data: ILoginRequest) =>
    async (dispatch: Dispatch) => {
        try {
            dispatch(loginStart());
            const res = await api.auth.login(data);
            dispatch(loginSuccess(res.data.token));
            window.location.href = "/history";
        } catch (e: any) {
            console.error(e);
            dispatch(loginFailure(e.message));
        }
    }

export const logoutUser = () =>
    async (dispatch: Dispatch) => {
        try {
            await api.auth.logout();
            localStorage.setItem('token', "");
            dispatch(resetAuth());
            window.location.href = "/auth/login";
        } catch (e: any) {
            console.error(e);
            dispatch(loginFailure(e.message));
        }
    }

