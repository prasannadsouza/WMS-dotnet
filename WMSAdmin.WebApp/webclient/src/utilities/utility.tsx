import { AppData, AppState, SessionData, ApplicationConfig, PaginationConfig, ConfigTimeStamp } from "../entities/configs"
import { ConfirmModel, LoginModel, MessageModel } from "../entities/models"
import { Locale } from "./locale"
import LocalizedStrings from "react-localization";
import { ErrorConstants, LinkConstants,CacheConstants, APIParts } from "../entities/constants";
import { ResponseData } from "../entities/entities";

export class Utility {

       static async GetData<T>(urlPart: string, searchParams:URLSearchParams,  defaultData: ResponseData<T>): Promise<ResponseData<T>> {
           const url: URL = new URL(window.origin + "/" + urlPart);

           const search = searchParams?.toString(); 
           if ((search?.length > 0) == true) url.search = search;
                      
           console.log(url.toString());
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
            return await Promise.resolve(response);
        };
        
        return await Utility.GetData<ApplicationConfig>(APIParts.CONFIG + "GetApplicationConfig", undefined, { data: null }).then(response => {
            if ((response.errors?.length > 0) === true) return response;
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
            return await Promise.resolve(response);
        };

        return await Utility.GetData<PaginationConfig>(APIParts.CONFIG +  "GetPaginationConfig", undefined, { data: null }).then(response => {
            if ((response.errors?.length > 0) === true) return response;
            Utility.saveToLocalStorage(CacheConstants.PAGINATIONCONFIG, response.data);
            return response;
        });
    }

    static async getAppData(): Promise<AppData> {

        let responseData: AppData = { appInitErrrors: [] }; 

        await Utility.GetApplicationConfig().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.applicationConfig = x.data;
        });

        await Utility.GetPaginationConfig().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.paginationConfig = x.data;
        });

        const locale = new Locale();

        await locale.getGeneralString().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.generalLocaleString = x.data;
        });

        await locale.getLanguages().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.languageCultures = x.data;
        });

        return await Promise.resolve(responseData);
    };

    static getAppState(appData: AppData, appState: AppState): AppState {
        let localizedGeneralString = new LocalizedStrings(appData.generalLocaleString);
        let newAppState: AppState = {
            generalString: localizedGeneralString,
            language: appData.languageCultures[0],
        };

        let localeCode = appState?.language?.code;

        if ((localeCode?.length > 0) === false) localeCode = appData.sessionData?.user?.localeCode;
        if ((localeCode?.length > 0) === false) localeCode = appData.sessionData?.customer?.localeCode
        if ((localeCode?.length > 0) === false) localeCode = appData.applicationConfig.localeCode;

        newAppState.language = appData.languageCultures.filter(
            (languageCulture) => languageCulture.code === localeCode
        )[0];

        newAppState.generalString.setLanguage(newAppState.language.code);
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
            title: appState.generalString?.confirmTitle,
            cancelTitle: appState.generalString?.no,
            message: appState.generalString?.confirmMessage,
            confirmTitle: appState.generalString?.yes,
            onClose: undefined,
            show: false,
        };
    };

    static getMessageModel = (appState: AppState): MessageModel => {
        return {
            title: appState.generalString?.message,
            okTitle: appState.generalString?.ok,
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
}
