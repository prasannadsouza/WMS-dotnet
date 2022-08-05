//Reference: https://blog.logrocket.com/persist-state-redux-persist-redux-toolkit-react/
// npm debounce (for delaying repeated actions)

import { useState } from 'react';
import { createContainer, createTrackedSelector } from 'react-tracked';
import { Utility } from './utility';
import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector } from 'react-redux';
import { AppConfig, AppData, SessionConfig, SystemConfig } from '../entities/configs';
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { LoginModel } from '../entities/models';
import { AppConstants, AppReducerConstants } from '../entities/constants';
import { persistStore, persistReducer } from 'redux-persist'
import storage from 'redux-persist/lib/storage' // defaults to localStorage for web
import thunk from 'redux-thunk';

const initialAppData = Utility.getAppData();

export const AppSlice = createSlice({
    name: AppReducerConstants.APP,
    initialState: initialAppData,
    reducers: {
        setAppModel: (state, action: PayloadAction<AppConfig>) => {
            return { ...state, appConfig: action.payload };
        },
        setLoginModel: (state, action: PayloadAction<LoginModel>) => {
            return { ...state, loginModel: action.payload };
        },
        setSession: (state, action: PayloadAction<SessionConfig>) => {
            return { ...state, appConfig: { ...state.appConfig, session: action.payload  } };
        },
        setSystem: (state, action: PayloadAction<SystemConfig>) => {
            return { ...state, appConfig: { ...state.appConfig, system: action.payload } };
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

const useInitialState = () => useState(Utility.getAppState(initialAppData.appConfig,null));
export const { Provider: GlobalStateProvider, useTrackedState: useTrackedGlobalState, useUpdate: useUpdateGlobalState, } =
    createContainer(useInitialState,);