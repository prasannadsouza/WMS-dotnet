import { AppConfig, AppData, AppState, SessionConfig, SystemConfig } from "../entities/configs"
import { ConfirmModel, MessageModel, ResponseModel } from "../entities/models"
import { Locale } from "./locale"
import LocalizedStrings from "react-localization";
import { AppConstants, ErrorConstants, LinkConstants } from "../entities/constants";
import { UIHelper } from "./uihelper";

export class Utility {

    static getAppData(): AppData {

        const appData: AppData = Utility.getLocalStorage();
        if (appData !== undefined && appData !== null) return appData;

        return {
            appConfig: Utility.getAppConfig(),
            loginModel: UIHelper.getInitialLoginModel(),
        };
    };

    static getAppConfig() {
        const appConfig: AppConfig = {
            system: Utility.getSystemConfig(),
        };
        return appConfig;
    };

    static getAppState(appConfig: AppConfig, appState: AppState): AppState {
        
        let newAppState: AppState = {
            globalStrings: new LocalizedStrings(appConfig.system.globalLocaleStrings),
            validationStrings: new LocalizedStrings(appConfig.system.validationLocaleStrings),
            messageStrings: new LocalizedStrings(appConfig.system.messageLocaleStrings),
            language: appConfig.system!.languages[0],
        };

        let  localeCode = appState?.language?.code;        

        if ((localeCode?.length > 0) === false) localeCode = appConfig.session?.user?.localeCode;
        if ((localeCode?.length > 0) === false) localeCode = appConfig.session?.customer?.localeCode
        if ((localeCode?.length > 0) === false) localeCode = appConfig.system.defaultLocaleCode;

        newAppState.language = appConfig.system!.languages!.filter(
            (language) => language.code === localeCode
        )[0];

        newAppState.globalStrings.setLanguage(newAppState.language.code);
        newAppState.validationStrings.setLanguage(newAppState.language.code);
        newAppState.messageStrings.setLanguage(newAppState.language.code);
        return newAppState;
    };

    static getSystemConfig() {
        const locale = new Locale();

        const systemConfig: SystemConfig = {
            appTitle: "WMS",
            defaultLocaleCode: "se",
            languages: locale.getLanguages(),
            validationLocaleStrings: locale.getValidationStrings(),
            globalLocaleStrings: locale.getGlobalStrings(),
            messageLocaleStrings: locale.getMessageStrings(),
        };
        return systemConfig;
    }

    static getSessionConfig(username: string, password: string): SessionConfig {
        const sessionConfig: SessionConfig = {
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
            title: appState.globalStrings?.confirmTitle,
            cancelTitle: appState.globalStrings?.no,
            message: appState.globalStrings?.confirmMessage,
            confirmTitle: appState.globalStrings?.yes,
            onClose: undefined,
            show: false,
        };
    };

    static getMessageModel = (appState: AppState): MessageModel => {
        return {
            title: appState.globalStrings?.message,
            okTitle: appState.globalStrings?.ok,
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
            default:
                return "/";
        }
    }

    static isUserLoggedIn = (appConfig: AppConfig): boolean => {
        if (appConfig?.session?.customer === undefined || appConfig?.session?.customer === null) return false;
        if (appConfig?.session?.user === undefined || appConfig?.session?.user === null) return false;
        return true;
    }

    static performUserLogin = (appState: AppState, username: string, password: string): ResponseModel<boolean> => {

        let response: ResponseModel<boolean> = {
            errors: []
        }

        if ((username?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.USERNAME_CANNOTBE_BLANK,
                message: appState.validationStrings?.usernameCannotBeBlank,
            });
        }

        if ((password?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.PASSWORD_CANNOTBE_BLANK,
                message: appState.validationStrings?.passwordCannotBeBlank,
            });
        }

        if (username === "prasanna") {
            response.errors?.push({
                errorCode: ErrorConstants.USERNAME_OR_PASSWORD_ISINVALID,
                message: appState.validationStrings?.usernameOrPasswordIsInvalid,
            });
        }

        if ((response.errors!.length > 0) === true) return response;
        response.data = true;
        return response;
    }

    static getUserLoginAppConfig = (appState: AppState, appConfig: AppConfig, username: string, password: string): AppConfig => {
        var newAppConfig = Utility.getAppConfig();
        newAppConfig.session = Utility.getSessionConfig(username, password);
        return newAppConfig;
    };

    static validateResetPassword = (appState: AppState, email: string): ResponseModel<boolean> => {
        let response: ResponseModel<boolean> = {
            data: false,
            errors: []
        }

        if ((email?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.EMAIL_CANNOTBE_BLANK,
                message: appState.validationStrings?.emailCannotBeBlank,
            });
        }

        response.data = true;
        return response;

    }

    static getOtherMenuTitle = (appConfig: AppConfig, appState: AppState) => {

        if (Utility.isUserLoggedIn(appConfig) !== true) return appState?.language?.name;

        let title = appConfig.session!.user!.firstName!;

        if (title!.length > 0) {
            title = title + " " + appConfig.session!.user!.lastName!;
        }

        if (title!.length > 0) {
            title = title + " (" + appConfig.session!.customer!.name! + ")"
        }

        return title;
    };

    static getLocalStorage = () => {
        // try {
        const serializedState = localStorage.getItem(AppConstants.KEY);
        if (!serializedState) return undefined;
        return JSON.parse(serializedState);
        // } catch (e) {
        //     return undefined;
        // }
    }

    static saveLocalStorage = (state: AppData) => {
        // try {
        const serializedState = JSON.stringify(state);
        localStorage.setItem(AppConstants.KEY, serializedState);
        // } catch (e) {
        //     // Ignore
        // }
    }
}
