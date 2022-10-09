import { ErrorData, Pagination } from "../entities/entities"

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

export type ResponseModel<T> = {
    data?: T;
    message?: string;
    pagination?: Pagination;
    errors?: ErrorData[];
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
};

