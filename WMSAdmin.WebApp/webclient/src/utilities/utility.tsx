import LocalizedStrings from "react-localization";
import { AppData, ApplicationConfig, AppState, ConfigTimeStamp, PaginationConfig, SessionData } from "../entities/configs";
import { APIParts, CacheConstants, ClientErrorConstants, LinkConstants, ServerErrorConstants } from "../entities/constants";
import { ErrorData, ResponseData, AuthenticateAppUserResponse } from "../entities/entities";
import { Locale } from "./locale";
import { UIHelper } from "./uihelper";
import _ from "lodash"; 
import { ActionCreatorWithOptionalPayload, AnyAction, Dispatch } from "@reduxjs/toolkit";

export class Utility {

    static async GetData<T>(urlPart: string, searchParams: URLSearchParams, defaultData: ResponseData<T>): Promise<ResponseData<T>> {
        const url: URL = new URL(window.origin + "/" + urlPart);

        const search = searchParams?.toString();
        if ((search?.length > 0) === true) url.search = search;

        let response = await fetch(url);

        if (!response.ok) {
            defaultData.errors = [{ errorCode: ClientErrorConstants.FETCH_GET, message: response.statusText }];
            return defaultData;
        }

        defaultData = response.json() as ResponseData<T>;
        return defaultData;

    }

    static async PostData<T>(urlPart: string, searchParams: URLSearchParams, data: object, defaultData: ResponseData<T>): Promise<ResponseData<T>> {
        const url: URL = new URL(window.origin + "/" + urlPart);
        const search = searchParams?.toString();
        if ((search?.length > 0) === true) url.search = search;

        const payLoad = JSON.stringify(data);

        let response = await fetch(url, {
            method: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json'
            },
            body: payLoad
        });

        if (!response.ok) {
            defaultData.errors = [{ errorCode: ClientErrorConstants.FETCH_POST, message: response.statusText }];
            return defaultData;
        }

        defaultData = response.json() as ResponseData<T>;
        return defaultData;

    }

    static async GetApplicationConfig(): Promise<ResponseData<ApplicationConfig>> {
        const data: ApplicationConfig = Utility.getFromLocalStorage(CacheConstants.APPLICATIONCONFIG);
        if (data !== undefined && data !== null) {
            let response: ResponseData<ApplicationConfig> = {
                data: data,
                errors: []
            }
            return await Promise.resolve(response);
        };

        let response = await Utility.GetData<ApplicationConfig>(APIParts.CONFIG + "GetApplicationConfig", undefined, { data: null });
        if ((response.errors?.length > 0) === true) return response;

        Utility.saveToLocalStorage(CacheConstants.APPLICATIONCONFIG, response.data);
        return response;
    }

    static async GetPaginationConfig(): Promise<ResponseData<PaginationConfig>> {
        const data: PaginationConfig = Utility.getFromLocalStorage(CacheConstants.PAGINATIONCONFIG);
        if (data !== undefined && data !== null) {
            let response: ResponseData<PaginationConfig> = {
                data: data,
                errors: []
            }
            return await Promise.resolve(response);
        };

        let response = await Utility.GetData<PaginationConfig>(APIParts.CONFIG + "GetPaginationConfig", undefined, { data: null });
        if ((response.errors?.length > 0) === true) return response;

        Utility.saveToLocalStorage(CacheConstants.PAGINATIONCONFIG, response.data);
        return await Promise.resolve(response);
    }

    static async getAppData(): Promise<AppData> {

        let responseData: AppData = {
            appInitErrrors: [], sessionData: {} }; 

        let appConfigResponse = await await Utility.GetApplicationConfig();
        
        if ((appConfigResponse.errors?.length > 0) === true) {
            responseData.appInitErrrors.push(...appConfigResponse.errors);
            return responseData;
        }

        responseData.applicationConfig = appConfigResponse.data;

        let languageResponse = await Locale.getLanguages();

        if ((languageResponse.errors?.length > 0) === true) {
            responseData.appInitErrrors.push(...languageResponse.errors);
            return responseData;
        }

        responseData.languageCultures = languageResponse.data;
        responseData.sessionData.language = languageResponse.data.filter((item) => { return item.code === responseData.applicationConfig.localeCode })[0];

        let paginationResponse = await Utility.GetPaginationConfig();
        if ((paginationResponse.errors?.length > 0) === true) {
            responseData.appInitErrrors.push(...paginationResponse.errors);
            return responseData;
        }

        responseData.paginationConfig = paginationResponse.data;

        let generalStringResponse = await Locale.getGeneralString();
        if ((generalStringResponse.errors?.length > 0) === true) {
            responseData.appInitErrrors.push(...generalStringResponse.errors);
            return responseData;
        }
        responseData.generalLocaleString = generalStringResponse.data;
        return await Promise.resolve(responseData);
    };

    static getAppState(appData: AppData): AppState {
        let localizedGeneralString = new LocalizedStrings(appData.generalLocaleString);
        let newAppState: AppState = {
            generalString: localizedGeneralString,
        };

        newAppState.generalString.setLanguage(Locale.getLocalizedLocaleCode(appData.sessionData.language.code));
        return newAppState;
    };

    static handleErrors = (appState: AppState, errors: ErrorData[], updateAppConfig: (value: React.SetStateAction<AppState>) => void, onClose: () => void = undefined): boolean => {
        if ((errors?.length > 0) === false) return false;
        UIHelper.showMessageModal(updateAppConfig, appState, true, errors[0].message, onClose);
        Utility.showLoader(updateAppConfig, false);
        return true;
    };

    static handleAuthRevalidation = (errors: ErrorData[], dispatch: Dispatch<AnyAction>, setAuthenticated: ActionCreatorWithOptionalPayload<boolean, string>, updateAppConfig: (value: React.SetStateAction<AppState>) => void, onClose: () => void = undefined): boolean => {
        if ((errors?.length > 0) === false) return false;
        const authReValidationErrors = errors?.filter((item) => item.errorCode == ServerErrorConstants.AUTHREVALIDATIONREQUIRED);
        if (authReValidationErrors?.length > 0 === true) {
            dispatch(setAuthenticated(false));
            Utility.showLoader(updateAppConfig, false);
            return true;
        }
        return false;
    }

    static handleAllErrors = (appState: AppState, errors: ErrorData[], dispatch: Dispatch<AnyAction>, setAuthenticated: ActionCreatorWithOptionalPayload<boolean, string>, updateAppConfig: (value: React.SetStateAction<AppState>) => void, onClose: () => void = undefined): boolean => {
        if ((errors?.length > 0) === false) return false;
        if (Utility.handleAuthRevalidation(errors, dispatch, setAuthenticated, updateAppConfig) === true) return true;
        return Utility.handleErrors(appState, errors, updateAppConfig, onClose);
    }


    static showLoader = (updateAppConfig: (value: React.SetStateAction<AppState>) => void, show: boolean) => {
        updateAppConfig((prev) => ({ ...prev, showLoader: show, }));
    }
    
    static getSessionConfig(appData:AppData, data: AuthenticateAppUserResponse): SessionData {
        const sessionData: SessionData = {
            appCustomer: data.appCustomer,
            appCustomerUser: data.appCustomerUser,
            language: appData.sessionData.language, 
        };
       
        let languageCode = Locale.getLocalizedLocaleCode(sessionData.appCustomerUser.localeCode);
        if ((languageCode?.length > 0) === false) languageCode = sessionData.appCustomer.localeCode;
        if ((languageCode?.length > 0) === false) languageCode = sessionData.language.code;

        sessionData.language = appData.languageCultures.filter(x => x.code === languageCode)[0];
        return sessionData;
    };

    
    static getLink(key: string) {
        switch (key) {
            case LinkConstants.HOME:
                return "/home";
            case LinkConstants.SETTINGS:
                return "/settings";
            case LinkConstants.LOGIN:
                return "/login";
            case LinkConstants.FETCHDATA:
                return "/fetchdata";
            default:
                return "/";
        }
    }

    static isUserLoggedIn = (appData: AppData): boolean => {
        if (appData?.sessionData?.isAuthenticated !== true) return false;
        if (appData?.sessionData?.appCustomer === undefined || appData?.sessionData?.appCustomer === null) return false;
        if (appData?.sessionData?.appCustomerUser === undefined || appData?.sessionData?.appCustomerUser === null) return false;
        return true;
    }

    static getOtherMenuTitle = (appData: AppData) => {

        if (Utility.isUserLoggedIn(appData) !== true) return appData.sessionData.language.name;

        let title = appData.sessionData!.appCustomerUser!.displayName!;

        if (title!.length > 0) {
            title = title + " (" + appData.sessionData!.appCustomer!.customerName! + ")"
        }

        return title;
    };

    static getFromLocalStorage = <T,>(key: string) => {
        // try {
        const serializedState = localStorage.getItem(key);
        if (!serializedState) return undefined;
        return JSON.parse(serializedState) as T;
        // } catch (e) {
        //     return undefined;
        // }
    }

    static saveToLocalStorage = <T,>(key: string, data: T) => {
        // try {
        const serializedState = JSON.stringify(data);
        localStorage.setItem(key, serializedState);
        this.saveConfigTimeStamp(key);
    }

    static removeFromLocalStorage = (key: string) => {
        let serializedState = localStorage.getItem(key);
        if (!serializedState) return;
        localStorage.removeItem(key);
    }

    static removeConfigTimeStamp = (key: string) => {

        let serializedState = localStorage.getItem(CacheConstants.CONFIGTIMESTAMPS);
        if (!serializedState) return;
        let data = JSON.parse(serializedState) as ConfigTimeStamp[];
        if ((data?.length > 0) !== true) return;

        data = data.filter(function (item) {
            return item.Code !== key
        });

        if ((data.length > 0) !== true) {
            localStorage.removeItem(CacheConstants.CONFIGTIMESTAMPS);
            return;
        }

        serializedState = JSON.stringify(data);
        localStorage.setItem(CacheConstants.CONFIGTIMESTAMPS, serializedState);
    }

    static getConfigTimeStamp = (key: string): ConfigTimeStamp => {

        let serializedState = localStorage.getItem(CacheConstants.CONFIGTIMESTAMPS);
        if (!serializedState) return undefined;
        let data = JSON.parse(serializedState) as ConfigTimeStamp[];
        if ((data?.length > 0) !== true) return undefined;

        const items = data?.filter(function (item) {
            return item.Code === key
        });

        if ((items?.length > 0) !== true) return undefined;
        return items[0];
    }

    static saveConfigTimeStamp = (cacheKey: string) => {

        let data: ConfigTimeStamp[] | undefined = undefined; 

        let serializedState = localStorage.getItem(CacheConstants.CONFIGTIMESTAMPS);
        if (serializedState)  data = JSON.parse(serializedState) as ConfigTimeStamp[];
        if (!data) data = [];
        
        const items = data?.filter(function (item) {
            return item.Code === cacheKey
        });

        let item: ConfigTimeStamp | undefined = undefined;
        if (items?.length > 0) item = items[0];

        if (item === undefined) {
            item = { Code: cacheKey }
            data?.push(item);
        }

        item.timeStamp = new Date(Date.now());

        serializedState = JSON.stringify(data);
        localStorage.setItem(CacheConstants.CONFIGTIMESTAMPS, serializedState);
    }

    static clearConfigCache = () => {
        let serializedState = localStorage.getItem(CacheConstants.CONFIGTIMESTAMPS);
        if (!serializedState) return;
        let data = JSON.parse(serializedState) as ConfigTimeStamp[];
        if ((data?.length > 0) !== true) return;

        data.forEach((configTimeStamp) => {
            this.removeFromLocalStorage(configTimeStamp.Code);
        });

        this.removeFromLocalStorage(CacheConstants.CONFIGTIMESTAMPS);
    }

    static copyObjectData = <T,>(theObject: T) => {
        return JSON.parse(JSON.stringify(theObject)) as T;
    }

    static cloneDeep = <T,>(theObject: T) => {
        return _.cloneDeep(theObject);
    }
}
