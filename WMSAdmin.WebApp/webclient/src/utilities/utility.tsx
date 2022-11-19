import { AppData, AppState, SessionData, ApplicationConfig, PaginationConfig, ConfigTimeStamp } from "../entities/configs"
import { ConfirmModel, MessageModel } from "../entities/models"
import { Locale } from "./locale"
import LocalizedStrings from "react-localization";
import { ErrorConstants, LinkConstants,CacheConstants } from "../entities/constants";
import { ResponseData } from "../entities/entities";
import { URL } from "url";

export class Utility {

    static async GetData<T>(url: URL, defaultData: ResponseData<T>): Promise<ResponseData<T>> {
        
        return await fetch(url).then(response => {
            if (!response.ok) {
                defaultData.errors = [{ errorCode: ErrorConstants.FETCH_GET, message: response.statusText }];
                return defaultData;
            }

            defaultData = response.json() as ResponseData<T>;
            return defaultData;
        });
    }

    static async GetApplicationConfig(): Promise<ResponseData<ApplicationConfig>> {
        const data: ApplicationConfig = Utility.getFromLocalStorage(CacheConstants.APPLICATIONCONFIG);
        if (data !== undefined && data !== null) {
            let response: ResponseData<ApplicationConfig> = {
                data: data,
                errors: []
            }
            return Promise.resolve(response);
        };

        return await this.GetData<ApplicationConfig>(new URL("api/Config/GetApplicationConfig"), { data: null }).then(response => {
            if (response.errors?.length > 0 == true) return response;

            
            Utility.saveToLocalStorage(CacheConstants.APPLICATIONCONFIG, response.data);
            return response;
        });
    }

    static async GetPaginationConfig(): Promise<ResponseData<PaginationConfig>> {
        const data: PaginationConfig = Utility.getFromLocalStorage(CacheConstants.PAGINATIONCONFIG);
        if (data !== undefined && data !== null) {
            let response: ResponseData<PaginationConfig> = {
                data: data,
                errors: []
            }
            return Promise.resolve(response);
        };

        return await this.GetData<PaginationConfig>(new URL("api/Config/GetPaginationConfig"), { data: null }).then(response => {
            if (response.errors?.length > 0 == true) return response;
            Utility.saveToLocalStorage(CacheConstants.PAGINATIONCONFIG, response.data);
            return response;
        });
    }



    static async getAppData(): Promise<AppData> {

        let responseData: AppData = { appInitErrrors: [] }; 

        Utility.GetApplicationConfig().then(x => {
            if (x.errors?.length > 0 == true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.applicationConfig = x.data;
        });

        Utility.GetPaginationConfig().then(x => {
            if (x.errors?.length > 0 == true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.paginationConfig = x.data;
        });

        const locale = new Locale();

        locale.getGeneralString().then(x => {
            if (x.errors?.length > 0 == true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.generalLocaleString = x.data;
        });

        return Promise.resolve(responseData);
    };

    static getNewAppData(appData: AppData,) {
        const newAppData: AppData = {
            applicationConfig: appData.applicationConfig,
        };
        return newAppData;
    };



    static getAppState(appConfig: AppData, appState: AppState): AppState {

        let newAppState: AppState = {
            GeneralString: new LocalizedStrings(appConfig.generalLocaleString),
            LoginString: new LocalizedStrings(appConfig.loginLocaleString),
            language: appConfig.languageCultures[0],
        };

        let localeCode = appState?.language?.code;

        if ((localeCode?.length > 0) === false) localeCode = appConfig.sessionData?.user?.localeCode;
        if ((localeCode?.length > 0) === false) localeCode = appConfig.sessionData?.customer?.localeCode
        if ((localeCode?.length > 0) === false) localeCode = appConfig.applicationConfig.localeCode;

        newAppState.language = appConfig.languageCultures.filter(
            (languageCulture) => languageCulture.code === localeCode
        )[0];

        newAppState.GeneralString.setLanguage(newAppState.language.code);
        newAppState.LoginString.setLanguage(newAppState.language.code);
        return newAppState;
    };

    

    static getSessionConfig(username: string, password: string): SessionData {
        const sessionConfig: SessionData = {
            customer: {
                name: "C-" + username,
                number: "123456",
                organizationNumber: "555555-55555",
                id: 1,
                localeCode: "se"
            },
            user: {
                firstName: username,
                lastName: password,
                localeCode: "en",
            },
        };

        return sessionConfig;
    };

    static getConfirmModel = (appState: AppState): ConfirmModel => {
        return {
            title: appState.GeneralString?.confirmTitle,
            cancelTitle: appState.GeneralString?.no,
            message: appState.GeneralString?.confirmMessage,
            confirmTitle: appState.GeneralString?.yes,
            onClose: undefined,
            show: false,
        };
    };

    static getMessageModel = (appState: AppState): MessageModel => {
        return {
            title: appState.GeneralString?.message,
            okTitle: appState.GeneralString?.ok,
            onClose: undefined,
            show: false,
            isError: false,
            message: undefined,
        };
    };

    static getLink(key: string) {
        switch (key) {
            case LinkConstants.HOME:
                return "/";
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
        if (appData?.sessionData?.customer === undefined || appData?.sessionData?.customer === null) return false;
        if (appData?.sessionData?.user === undefined || appData?.sessionData?.user === null) return false;
        return true;
    }

    static performUserLogin = (appState: AppState, username: string, password: string): ResponseData<boolean> => {

        let response: ResponseData<boolean> = {
            errors: []
        }

        if ((username?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.USERNAME_CANNOTBE_BLANK,
                message: appState.LoginString?.usernameCannotBeBlank,
            });
        }

        if ((password?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.PASSWORD_CANNOTBE_BLANK,
                message: appState.LoginString?.passwordCannotBeBlank,
            });
        }

        if (username === "prasanna") {
            response.errors?.push({
                errorCode: ErrorConstants.USERNAME_OR_PASSWORD_ISINVALID,
                message: appState.LoginString?.usernameOrPasswordIsInvalid,
            });
        }

        if ((response.errors!.length > 0) === true) return response;
        response.data = true;
        return response;
    }

    static getUserLoginAppConfig = (appState: AppState, appConfig: AppData, username: string, password: string): AppData => {
        var newAppData = Utility.getNewAppData(appConfig);
        newAppData.sessionData = Utility.getSessionConfig(username, password);
        return newAppData;
    };

    static validateResetPassword = (appState: AppState, email: string): ResponseData<boolean> => {
        let response: ResponseData<boolean> = {
            data: false,
            errors: []
        }

        if ((email?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.EMAIL_CANNOTBE_BLANK,
                message: appState.LoginString?.emailCannotBeBlank,
            });
        }

        response.data = true;
        return response;

    }

    static getOtherMenuTitle = (appData: AppData, appState: AppState) => {

        if (Utility.isUserLoggedIn(appData) !== true) return appState?.language?.name;

        let title = appData.sessionData!.user!.firstName!;

        if (title!.length > 0) {
            title = title + " " + appData.sessionData!.user!.lastName!;
        }

        if (title!.length > 0) {
            title = title + " (" + appData.sessionData!.customer!.name! + ")"
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
        if (data?.length > 0 !== true) return;

        data = data.filter(function (item) {
            return item.Code !== key
        });

        if (data.length > 0 !== true) {
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
        if (data?.length > 0 !== true) return undefined;

        const items = data?.filter(function (item) {
            return item.Code == key
        });

        if (items?.length > 0 !== true) return undefined;
        return items[0];
    }

    static saveConfigTimeStamp = (cacheKey: string) => {

        let data: ConfigTimeStamp[] | undefined = undefined; 

        let serializedState = localStorage.getItem(CacheConstants.CONFIGTIMESTAMPS);
        if (serializedState)  data = JSON.parse(serializedState) as ConfigTimeStamp[];
        if (!data) data = [];
        
        const items = data?.filter(function (item) {
            return item.Code == cacheKey
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
}
