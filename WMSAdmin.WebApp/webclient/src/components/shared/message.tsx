import { useEffect, useRef } from 'react';
import { useTrackedGlobalState, useUpdateGlobalState } from '../../utilities/store';
import { UIHelper } from '../../utilities/uihelper';
import { Utility } from '../../utilities/utility';


export const Message = () => {
    const updateAppConfig = useUpdateGlobalState();
    const appConfig = useTrackedGlobalState();
    const model = appConfig.messageModel;
    const generalString = appConfig.GeneralString;
    let okButton = useRef<HTMLButtonElement>(null);
    let showModal = model?.show === true;

    const handleClose = () => {
        if (model?.onClose !== undefined && model?.onClose !== null) model.onClose();
        updateAppConfig((prev) => ({ ...prev, messageModel: Utility.getMessageModel(appConfig) }));
    }

    const getClassName = () => {
        return " " + (model?.isError === true ? "alert-danger" : "alert-success") + " ";
    }

    useEffect(() => {
        if (showModal === true) {
            okButton.current?.focus();
        }

    }, [model?.show]);

    return (
        <div className={"modal" + UIHelper.getModalClass(model?.show)} tabIndex={-1} role="dialog">
            <div className="modal-dialog" role="document">
                <div className="modal-content mt-5 border border-warning">
                    <div className={"modal-header" + getClassName()}>
                        <h5 className="modal-title">{model?.title ?? (model?.isError === true ? generalString?.error : generalString?.message)}</h5>
                        <button ref={okButton} type="button" className="btn btn-close border border-dark" onClick={() => handleClose()} data-bs-dismiss="modal" aria-label={model?.okTitle ?? generalString?.ok}></button>
                    </div>
                    <div className="modal-body">
                        <p>{model?.message ?? (model?.isError === true ? generalString?.error : generalString?.message)}</p>
                    </div>
                    <div className="modal-footer">
                        <div className='d-flex justify-content.end'>
                            <button type="button" onClick={() => handleClose()} className="btn btn-primary" data-bs-dismiss="modal">{model?.okTitle ?? generalString?.ok}</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}