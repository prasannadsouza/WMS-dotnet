import LocalizedStrings from "react-localization";
import App from "../App";
import { AppData, ApplicationConfig, AppState, ConfigTimeStamp, PaginationConfig, SessionData } from "../entities/configs";
import { APIParts, CacheConstants, ErrorConstants, LinkConstants } from "../entities/constants";
import { ErrorData, ResponseData } from "../entities/entities";
import { ConfirmModel, MessageModel } from "../entities/models";
import { Locale } from "./locale";
import { UIHelper } from "./uihelper";

export class Utility {

    static async GetData<T>(urlPart: string, searchParams: URLSearchParams, defaultData: ResponseData<T>): Promise<ResponseData<T>> {
        const url: URL = new URL(window.origin + "/" + urlPart);

        const search = searchParams?.toString();
        if ((search?.length > 0) === true) url.search = search;
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

        let responseData: AppData = {
            appInitErrrors: [], sessionData: {} }; 

        await Utility.GetApplicationConfig().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return responseData;
            }
            responseData.applicationConfig = x.data;
            return responseData;
        }).then(async (response) => {
            if ((response.appInitErrrors?.length > 0) === true) return;

            await Locale.getLanguages().then(x => {
                if ((x.errors?.length > 0) === true) {
                    responseData.appInitErrrors.push(...x.errors);
                    return;
                }
                responseData.languageCultures = x.data;
                responseData.sessionData.language = x.data.filter((item) => { return item.code == responseData.applicationConfig.localeCode })[0];
            });
        });

        await Utility.GetPaginationConfig().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.paginationConfig = x.data;
        });

        await Locale.getGeneralString().then(x => {
            if ((x.errors?.length > 0) === true) {
                responseData.appInitErrrors.push(...x.errors);
                return;
            }
            responseData.generalLocaleString = x.data;
        });

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
        UIHelper.showMessageModal(updateAppConfig,appState, true, errors[0].message, onClose);
        return true;
    };

    static showLoader = (updateAppConfig: (value: React.SetStateAction<AppState>) => void, show: boolean) => {
        updateAppConfig((prev) => ({ ...prev, showLoader: show, }));
    }

    
    static getSessionConfig(appData:AppData, username: string, password: string): SessionData {
        const sessionData: SessionData = {
            customer: {
                name: "C-" + username,
                number: "123456",
                organizationNumber: "555555-55555",
                id: 1,
                localeCode: "sv-SE"
            },
            user: {
                firstName: username,
                lastName: password,
                localeCode: "en-SE",
            },
            language: appData.sessionData.language, 
        };

        return sessionData;
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

    

    static getOtherMenuTitle = (appData: AppData) => {

        if (Utility.isUserLoggedIn(appData) !== true) return appData.sessionData.language.name;

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
}
