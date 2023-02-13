import { AppState } from "../entities/configs";
import { ConfirmModel } from "../entities/models";

export class UIHelper {
    static getclassIsInvalid = (message: string): string => {
        if ((message?.length > 0) === true) return " is-invalid "
        return "";
    }

    static getModalClass = (showModal?: boolean) => {
        if (showModal !== true) return "";
        return " modal-show ";
    }

    static showMessageModal = (updateAppConfig: (value: React.SetStateAction<AppState>) => void, appState: AppState, isError: boolean,
        message: string, onClose: () => void = undefined,
        title: string = undefined, okTitle: string = undefined) => {

        if ((okTitle?.length > 0) === false) okTitle = appState.generalString?.ok;

        if ((title?.length > 0) === false) {
            title = isError ? appState.generalString?.error : appState.generalString?.message
        }

        const model = {
            title: title,
            okTitle: okTitle,
            onClose: onClose,
            show: true,
            isError: isError,
            message: message,
        };

        updateAppConfig((prev) => ({ ...prev, messageModel: model }));
    }

    static showConfirmationModal = (updateAppConfig: (value: React.SetStateAction<AppState>) => void, appState: AppState, onClose: (confirmed: boolean) => void = undefined,
        title: string = undefined, cancelTitle: string = undefined, confirmTitle: string = undefined, message: string = undefined) => {
        if ((title?.length > 0) === false) title = appState.generalString?.confirmTitle;
        if ((cancelTitle?.length > 0) === false) title = appState.generalString?.no;
        if ((message?.length > 0) === false) title = appState.generalString?.confirmMessage;
        if ((confirmTitle?.length > 0) === false) title = appState.generalString?.confirmTitle;

        let model: ConfirmModel = {
            title: title,
            cancelTitle: cancelTitle,
            message: message,
            confirmTitle: confirmTitle,
            onClose: onClose,
            show: true,
        };

        updateAppConfig((prev) => ({ ...prev, confirmModel: model }));
    }
}