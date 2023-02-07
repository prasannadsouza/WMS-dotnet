import { LocalizedStrings } from "react-localization";
import { LoginString } from "./locales";

export type ConfirmModel = {
    title?: string;
    message?: string;
    confirmTitle?: string;
    cancelTitle?: string;
    onClose?: (confirmed: boolean) => void;
    show?: boolean;
}

export type MessageModel = {
    isError?: boolean;
    title?: string;
    message?: string;
    okTitle?: string;
    onClose?: () => void;
    show?: boolean;
}

export type LoginModel = {
    username?: string,
    usernameFeedBack?: string,
    password?: string,
    passwordFeedback?: string,
    showPassword?: boolean,
    showForgotPassword?: boolean,
    email?: string,
    emailFeedback?: string,
    loginString?: LocalizedStrings<LoginString>;
};