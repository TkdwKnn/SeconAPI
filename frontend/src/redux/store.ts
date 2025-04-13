import {configureStore} from "@reduxjs/toolkit";
import mainReducer from "./main.slice.ts";
import authReducer from "./auth.slice.ts";
import miscReducer from "./misc.slice.ts";

export const store = configureStore({
    reducer: {
        main: mainReducer,
        auth: authReducer,
        misc: miscReducer,
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware({serializableCheck: false}),
});


export type AppStore = typeof store;
export type RootState = ReturnType<AppStore['getState']>;
export type AppDispatch = AppStore['dispatch'];