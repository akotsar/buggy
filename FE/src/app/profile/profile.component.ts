import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { FORM_DIRECTIVES, REACTIVE_FORM_DIRECTIVES, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

import { ApiService } from '../shared/api.service';
import { LoginService } from '../shared/login.service';
import { UserProfile } from '../shared/models/user-profile';

@Component({
    moduleId: module.id,
    selector: 'my-profile',
    templateUrl: 'profile.component.html',
    directives: [ROUTER_DIRECTIVES, FORM_DIRECTIVES, REACTIVE_FORM_DIRECTIVES],
    providers: []
})
export class ProfileComponent implements OnInit {
    form: FormGroup;
    loading = true;
    error: string;
    success = false;
    sending = false;

    constructor(
        private fb: FormBuilder,
        private api: ApiService,
        private login: LoginService
    ) {
    }

    ngOnInit() {
        this.api.getProfile(this.login.getToken())
            .subscribe(p => this.initForm(p));
    }

    onSubmit() {
        this.error = '';
        this.success = false;
        this.sending = true;

        this.api.saveProfile(this.login.getToken(), this.form.value)
            .finally(() => this.sending = false)
            .subscribe(
                r => {
                    this.resetPasswords();
                    this.success = true;
                },
                e => this.error = e
            );
    }

    private resetPasswords() {
        for (let controlName of ['currentPassword', 'newPassword', 'newPasswordConfirmation']) {
            let control = <FormControl>this.form.controls[controlName];
            control.updateValue('');
            control.setErrors(null);
        }
    }

    private initForm(profile: UserProfile) {
        this.form = this.fb.group({
            'username': [profile.username],
            'firstName': [profile.firstName, Validators.required],
            'lastName': [profile.lastName, Validators.required],
            'gender': [profile.gender],
            'age': [profile.age, c => this.validateAge(c)],
            'address': [profile.address],
            'phone': [profile.phone],
            'hobby': [profile.hobby],
            'currentPassword': [''],
            'newPassword': [''],
            'newPasswordConfirmation': ['', c => this.validateConfirmPassword(c)]
        });

        this.form.controls['phone'].valueChanges.subscribe(v => this.sanitizePhone());

        this.form.controls['newPassword'].valueChanges.subscribe((value) => {
            this.form.controls['newPasswordConfirmation'].updateValueAndValidity();
        });

        this.loading = false;
    }

    private sanitizePhone() {
        let phone: string = this.form.controls['phone'].value || '';

        let sanitized = phone.replace(/[^0-9+()]/, '');
        if (sanitized !== phone) {
            (<FormControl>this.form.controls['phone']).updateValue(sanitized);
        }
    }

    private validateAge(c: FormControl) {
        /* tslint:disable */ // Keeping '==' intentionally
        let value = c.value && parseInt(c.value, 10);
        if (value && value == c.value && (value < 0 || value > 95)) {
            return {
                'age': {
                    value: false
                }
            };
        }

        /* tslint:enable */

        return null;
    }

    private validateConfirmPassword(c: FormControl) {
        if (this.form && this.form.controls['newPassword'].value !== c.value) {
            return {
                confirm: {
                    valid: false
                }
            };
        }

        return null;
    }
}
