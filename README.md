# GAME3121
GAME Engine Development 1 (Unity)

Week1
https://learn.unity.com/pathway/game-development/unit/planning-a-game/tutorial/game-genres?version=6.3

Week2
Github version control
https://learn.unity.com/course/collaborate-with-github-desktop/tutorial/set-up-git-lfs-to-manage-large-files-in-unity?version=6.2

https://docs.github.com/en/repositories/working-with-files/managing-large-files/configuring-git-large-file-storage

Open Terminal.

Change your current working directory to an existing repository you'd like to use with Git LFS.

To associate a file type in your repository with Git LFS, enter git lfs track followed by the name of the file extension you want to automatically upload to Git LFS.

For example, to associate a .psd file, enter the following command:

$ git lfs track "*.psd"
> Tracking "*.psd"
Every file type you want to associate with Git LFS will need to be added with git lfs track. This command amends your repository's .gitattributes file and associates large files with Git LFS.

Add a file to the repository matching the extension you've associated:

git add path/to/file.psd
Commit the file and push it to GitHub:

git commit -m "add file.psd"
git push
