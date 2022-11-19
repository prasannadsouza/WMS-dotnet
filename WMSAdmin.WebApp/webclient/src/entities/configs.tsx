import { LocalizedStrings } from "react-localization";
import { Customer, ErrorData, User } from "../entities/entities"
import { ConfirmModel, LoginModel, MessageModel } from "../entities/models"
import { LanguageCulture, GeneralString, LoginString, GeneralLocaleString, LoginLocaleString } from "./locales"


export type ApplicationConfig = {
    applicationTitle?: string;
    baseUrl?: string;
    currentVersion?: string;
    localeCode?: string;
    uiLocaleCode?:string
}

export type SessionData = {
    user?: User
    customer?: Customer;
} 

export type AppData = {
    loginModel?: LoginModel;
    appInitErrrors?: ErrorData[];
    applicationConfig?: ApplicationConfig;
    paginationConfig?: PaginationConfig;
    generalLocaleString?: GeneralLocaleString;
    loginLocaleString?: LoginLocaleString;
    languageCultures?: LanguageCulture[];
    sessionData?: SessionData;
}

export type AppState = {
    confirmModel?: ConfirmModel;
    messageModel?: MessageModel;
    GeneralString?: LocalizedStrings<GeneralString>;
    LoginString?: LocalizedStrings<LoginString>;
    currentTitle?: string;
    showLoader?: boolean;
    language?: LanguageCulture;
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


