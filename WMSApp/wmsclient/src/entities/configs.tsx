import { LocalizedStrings } from "react-localization";
import { Customer, User } from "../entities/entities"
import { ConfirmModel, LoginModel, MessageModel } from "../entities/models"
import { Language, GlobalStrings, ValidationStrings, MessageStrings, GlobalLocaleStrings, ValidationLocaleStrings, MessageLocaleStrings } from "./locales"

export type AppConfig = {
    system?: SystemConfig;
    session?: SessionConfig;
}
export type SystemConfig = {
    appTitle?: string;
    defaultLocaleCode?: string;
    languages?: Language[];
    globalLocaleStrings?: GlobalLocaleStrings;
    validationLocaleStrings?: ValidationLocaleStrings;
    messageLocaleStrings?: MessageLocaleStrings;
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
    globalStrings?: LocalizedStrings<GlobalStrings>;
    validationStrings?: LocalizedStrings<ValidationStrings>;
    messageStrings?: LocalizedStrings<MessageStrings>;
    currentTitle?: string;
    showLoader?: boolean;
    language?: Language;
}


