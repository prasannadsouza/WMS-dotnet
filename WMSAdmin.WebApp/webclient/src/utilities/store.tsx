//Reference: https://blog.logrocket.com/persist-state-redux-persist-redux-toolkit-react/
// npm debounce (for delaying repeated actions)

import { useState } from 'react';
import { createContainer, createTrackedSelector } from 'react-tracked';
import { Utility } from './utility';
import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from 'react-redux';
import { AppData, SessionData, ApplicationConfig, PaginationConfig } from '../entities/configs';
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoginModel } from '../entities/models';
import { AppConstants, AppReducerConstants } from '../entities/constants';
import { persistStore, persistReducer } from 'redux-persist'
import storage from 'redux-persist/lib/storage' // defaults to localStorage for web
import thunk from 'redux-thunk';
import { GeneralLocaleString, LanguageCulture, LoginLocaleString } from '../entities/locales';
import { ErrorData } from '../entities/entities';

 
const initialAppData: AppData = {};

export const AppSlice = createSlice({
    name: AppReducerConstants.APP,
    initialState: initialAppData,
    reducers: {
        setAppModel: (state, action: PayloadAction<AppData>) => {
            return { ...action.payload }
        },
        setLoginModel: (state, action: PayloadAction<LoginModel>) => {
            return { ...state, loginModel: action.payload };
        },
        setAppInitErrrors: (state, action: PayloadAction<ErrorData[]>) => {
            return { ...state, appInitErrrors: action.payload };
        },
        setApplicationConfig: (state, action: PayloadAction<ApplicationConfig>) => {
            return { ...state, applicationConfig: action.payload };
        },
        setPaginationConfig: (state, action: PayloadAction<PaginationConfig>) => {
            return { ...state, paginationConfig: action.payload };
        },
        setGeneralLocaleString: (state, action: PayloadAction<GeneralLocaleString>) => {
            return { ...state, generalLocaleString: action.payload  };
        },
        setloginLocaleString: (state, action: PayloadAction<LoginLocaleString>) => {
            return { ...state, loginLocaleString: action.payload };
        },
        setLanguageCulture: (state, action: PayloadAction<LanguageCulture[]>) => {
            return { ...state, languageCultures: action.payload };
        },
        setSessionData: (state, action: PayloadAction<SessionData>) => {
            return { ...state, sessionData: action.payload };
        },
        setSessionLocale: (state, action: PayloadAction<LanguageCulture>) => {
            return { ...state, sessionData: { ...state.sessionData, language: action.payload } };
        },
        setSessionIsAuthenticated: (state, action: PayloadAction<boolean>) => {
            return { ...state, sessionData: { ...state.sessionData, isAuthenticated: action.payload } };
        },
    },
});

const persistConfig = {
    key: AppConstants.KEY,
    storage,
}

const persistedReducer = persistReducer(persistConfig, AppSlice.reducer);

export const appStore = configureStore({
    reducer: persistedReducer, devTools: process.env.NODE_ENV !== 'production',
    middleware: [thunk] }); 
export const appPersistor = persistStore(appStore);
export const useAppDispatch = () => useDispatch<typeof appStore.dispatch>();
export const useAppTrackedSelector = createTrackedSelector<AppData>(useSelector);

const GetInitialState = () => {
    const appData = useAppTrackedSelector();
    return Utility.getAppState(appData);
}
const useInitialState = () => useState(GetInitialState());
export const { Provider: GlobalStateProvider, useTrackedState: useTrackedGlobalState, useUpdate: useUpdateGlobalState, } = createContainer(useInitialState,);