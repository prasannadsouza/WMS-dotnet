import { LocalizedStrings } from "react-localization";
import { Customer, User } from "../entities/entities"
import { ConfirmModel, LoginModel, MessageModel } from "../entities/models"
import { Language, GeneralString, LoginString, GeneralLocaleString, LoginLocaleString } from "./locales"

export type AppConfig = {
    system?: SystemConfig;
    session?: SessionConfig;
}
export type SystemConfig = {
    appTitle?: string;
    defaultLocaleCode?: string;
    languages?: Language[];
    GeneralLocaleString?: GeneralLocaleString;
    LoginLocaleString?: LoginLocaleString;
}

export type SessionConfig = {
    user?: User
    customer?: Customer;
}

export type AppData = {
    appConfig?: AppConfig;
    loginModel?: LoginModel;
}

export type AppState = {
    confirmModel?: ConfirmModel;
    messageModel?: MessageModel;
    GeneralString?: LocalizedStrings<GeneralString>;
    LoginString?: LocalizedStrings<LoginString>;
    currentTitle?: string;
    showLoader?: boolean;
    language?: Language;
}


