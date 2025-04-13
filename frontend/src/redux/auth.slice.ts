import {createSlice, PayloadAction} from "@reduxjs/toolkit";

const initialState = {
    authData: {
        accessToken: "" as string | null,
        isLoading: false,
        error: null as string | null,
    }
}

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        loginStart: (state) => {
            state.authData.isLoading = true;
        },
        loginSuccess: (state, action: PayloadAction<string>) => {
            state.authData.isLoading = false;
            state.authData.accessToken = action.payload;
            localStorage.setItem('token', action.payload);
            state.authData.error = null;
        },
        loginFailure: (state, action: PayloadAction<string>) => {
            state.authData.isLoading = false;
            state.authData.error = action.payload;
        },

        resetAuth: () => initialState
    }
})

export const {
    resetAuth, loginFailure, loginSuccess, loginStart
} = authSlice.actions;

export default authSlice.reducer;