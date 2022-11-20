export class UIHelper {
    static getclassIsInvalid = (message: string): string => {
        if ((message?.length > 0) === true) return " is-invalid "
        return "";
    }

    static getModalClass = (showModal?: boolean) => {
        if (showModal !== true) return "";
        return " modal-show ";
    }

    
}