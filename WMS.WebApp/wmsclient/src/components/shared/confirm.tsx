import { useTrackedGlobalState, useUpdateGlobalState } from '../../utilities/store';
import { UIHelper } from '../../utilities/uihelper';
import { Utility } from '../../utilities/utility';
import { useEffect, useRef } from 'react';


export const Confirm = () => {
    const updateAppConfig = useUpdateGlobalState();
    const appConfig = useTrackedGlobalState();
    const model = appConfig.confirmModel;
    const globalStrings = appConfig.globalStrings;
    let okButton = useRef<HTMLButtonElement>(null);
    let showModal = model?.show === true;

    const handleClose = (confirmed: boolean) => {
        if (model?.onClose !== undefined && model?.onClose !== null) model.onClose(confirmed);
        updateAppConfig((prev) => ({ ...prev, confirmModel: Utility.getConfirmModel(appConfig) }));
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
                    <div className="modal-header alert-warning">
                        <h5 className="modal-title">{model?.title?.trim() ?? globalStrings?.confirmTitle}</h5>
                        <button ref={okButton} type="button" onClick={(e) => handleClose(false)} className="btn btn-close border border-dark" data-bs-dismiss="modal" aria-label={model?.cancelTitle?.trim() ?? globalStrings?.no}></button>
                    </div>
                    <div className="modal-body">
                        <p>{model?.message?.trim() ?? globalStrings?.confirmMessage}</p>
                    </div>
                    <div className="modal-footer d-flex justify-content-around">
                        <button type="button" onClick={(e) => handleClose(true)} className="btn btn-primary" data-bs-dismiss="modal">{model?.confirmTitle?.trim() ?? globalStrings?.yes}</button>
                        <button type="button" onClick={(e) => handleClose(false)} className="btn btn-secondary" data-bs-dismiss="modal">{model?.cancelTitle?.trim() ?? globalStrings?.no}</button>
                    </div>
                </div>
            </div>
        </div>
    );
}