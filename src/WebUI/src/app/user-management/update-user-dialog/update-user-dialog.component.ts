import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';
import { ApplicationUserDto } from 'src/app/entities/applicationUser/applicationUserDto';

@Component({
  selector: 'app-update-user-dialog',
  templateUrl: './update-user-dialog.component.html',
  styleUrls: ['./update-user-dialog.component.css']
})
export class UpdateUserDialogComponent implements OnInit {
  updateUserForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    public dialogRef: MatDialogRef<UpdateUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ApplicationUserDto
  ) {
    this.updateUserForm = this.fb.group({
      firstName: [data.firstName, Validators.required],
      middleName: [data.middleName, Validators.required],
      lastName: [data.lastName, Validators.required],
      gender: [data.gender, Validators.required],
      specialty: [data.specialty],
      visitCost: [data.visitCost],
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.updateUserForm.valid) {
      const updatedUser: ApplicationUserDto = {
        id: this.data.id,
        ...this.updateUserForm.value,
      };
      
      this.userService.updateUser(updatedUser).subscribe(() => {
        this.dialogRef.close(updatedUser);
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  userIsDoctor(): boolean {
    return this.data.specialty !== undefined && this.data.specialty !== null && this.data.specialty !== '';
  }
}
