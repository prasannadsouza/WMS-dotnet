import { LocalizedStrings } from "react-localization";
import { AppCustomer, ErrorData, AppCustomerUser } from "../entities/entities"
import { ConfirmModel, LoginModel, MessageModel } from "../entities/models"
import { LanguageCulture, GeneralString, GeneralLocaleString } from "./locales"


export type ApplicationConfig = {
    applicationTitle?: string;
    baseUrl?: string;
    currentVersion?: string;
    localeCode?: string;
    uiLocaleCode?:string
}

export type SessionData = {
    appCustomerUser?: AppCustomerUser
    appCustomer?: AppCustomer;
    language?: LanguageCulture;
} 

export type AppData = {
    loginModel?: LoginModel;
    appInitErrrors?: ErrorData[];
    applicationConfig?: ApplicationConfig;
    paginationConfig?: PaginationConfig;
    generalLocaleString?: GeneralLocaleString;
    languageCultures?: LanguageCulture[];
    sessionData?: SessionData;
}

export type AppState = {
    confirmModel?: ConfirmModel;
    messageModel?: MessageModel;
    generalString?: LocalizedStrings<GeneralString>;
    currentTitle?: string;
    showLoader?: boolean;
}

export type PaginationConfig = {
    recordsPerPage?: number;
    maxRecordsPerPage?: number;
    totalPagesToJump?: number;
}

export type ConfigTimeStamp = {
    timeStamp?: Date,
    Code?: string
}


