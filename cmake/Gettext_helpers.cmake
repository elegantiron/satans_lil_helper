## Gettetxt helper function for cmake

## Usage:
#   add_gettext_domain(
#       DOMAIN <domain-name>
#       TARGET_NAME <target-name>
#       SOURCES <file> ...
#       POTFILE_DESTINATION <dir>
#       POFILE_DESTINATION <dir>
#       MOFILE_DESTINATION <dir>
#       LANGUAGES <file> ...
#       [ALL]
#       [INSTALL_DESTINATION <dest>]
#       [INSTALL_COMPONENT <dest>]
#       [XGETTEXT_ARGS <args> ...
#       [MSGMERGE_ARGS <args> ...]
#       [MSGINIT_ARGS <args> ...]
#       [MSGFMT_ARGS <args> ... ]
#   )

## Example [CMake]:
# # Creating gettext target for application [messages domain]
# add_gettext_domain(
#     DOMAIN messages
#     TARGET_NAME application_gettext_domain_messages
#     SOURCES ${APPLICATION_SOURCES} ${APPLICATION_HEADERS}
#     POTFILE_DESTINATION ${CMAKE_CURRENT_SOURCE_DIR}/${CMAKE_INSTALL_LOCALEDIR}
#     MOFILE_DESTINATION ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_LOCALEDIR}
#     INSTALL_DESTINATION ${CMAKE_INSTALL_LOCALEDIR}
#     LANGUAGES "pt_BR"
#     XGETTEXT_ARGS
#         "--keyword=_" "--keyword=n_" "--package-name=${PROJECT_NAME}" "--package-version=${PROJECT_VERSION}"
#         "--copyright-holder=Jhon Doe" "--msgid-bugs-address=jhon.doe@example.com"
# )
# add_dependencies(application application_gettext_domain_messages)
#
# # Creating gettext target for application [errors domain]
# add_gettext_domain(
#     DOMAIN errors
#     TARGET_NAME application_gettext_domain_errors
#     SOURCES ${APPLICATION_SOURCES} ${APPLICATION_HEADERS}
#     POTFILE_DESTINATION ${CMAKE_CURRENT_SOURCE_DIR}/${CMAKE_INSTALL_LOCALEDIR}
#     MOFILE_DESTINATION ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_LOCALEDIR}
#     INSTALL_DESTINATION ${CMAKE_INSTALL_LOCALEDIR}
#     LANGUAGES "pt_BR"
#     XGETTEXT_ARGS
#         "--keyword=_errors" "--keyword=n_errors" "--package-name=${PROJECT_NAME}" "--package-version=${PROJECT_VERSION}"
#         "--copyright-holder=Jhon Doe" "--msgid-bugs-address=jhon.doe@example.com"
# )
# add_dependencies(application application_gettext_domain_errors)

## Example [C/C++]:
# #include <stdlib.h>
# #include <libintl.h>
#
# /* Messages Domain */
# #define _(message) gettext(message)
# #define n_(message1, message2, count) ngettext(message1, message2, count)
#
# /* Errors Domain */
# #define _errors(message) dgettext("errors", message)
# #define n_errors(message1, message2, count) dngettext("errors", message1, message2, count)
#
# /* Application Entry Point */
# int main(int argc, char **argv)
# {
#     /* Setting the i18n environment */
#     setlocale(LC_ALL, "");
#     bindtextdomain("messages", CMAKE_INSTALL_LOCALEDIR);
#     bindtextdomain("errors", CMAKE_INSTALL_LOCALEDIR);
#     textdomain("messages");
#
#     /* i18n Testing */
#     printf(_("Hello, World!"));
#     printf("\n");
#
#     printf(_errors("This is an message from the 'errors' domain!"));
#     printf("\n");
#
#     return 0;
# }

# Include GNUInstallDirs
include(GNUInstallDirs)

# Find gettext executables
find_program(GETTEXT_XGETTEXT_COMMAND xgettext)
find_program(GETTEXT_MSGFMT_COMMAND   msgfmt)
find_program(GETTEXT_MSGINIT_COMMAND  msginit)
find_program(GETTEXT_MSGMERGE_COMMAND msgmerge)

function(add_gettext_domain)
    # Ensure the utility programs are available
    if(NOT GETTEXT_XGETTEXT_COMMAND OR
       NOT GETTEXT_MSGFMT_COMMAND OR
       NOT GETTEXT_MSGMERGE_COMMAND OR
       NOT GETTEXT_MSGINIT_COMMAND)
        message(FATAL_ERROR "[GETTEXT] Could not find required executables")
    endif()

    set(options
        ALL)

    set(one_value_args
        DOMAIN
        INSTALL_DESTINATION
        INSTALL_COMPONENT
        TARGET_NAME
        POTFILE_DESTINATION
        POFILE_DESTINATION
        MOFILE_DESTINATION)

    set(multi_args
        SOURCES
        LANGUAGES
        XGETTEXT_ARGS
        MSGFMT_ARGS
        MSGINIT_ARGS
        MSGMERGE_ARGS)

    cmake_parse_arguments(GETTEXT
        "${options}"
        "${one_value_args}"
        "${multi_args}"
        ${ARGV})

    if(NOT GETTEXT_DOMAIN)
        message(FATAL_ERROR "[GETTEXT] Must supply a DOMAIN")
    elseif(NOT GETTEXT_POTFILE_DESTINATION)
        message(FATAL_ERROR "[GETTEXT] Must supply a POTFILE_DESTINATION")
    elseif(NOT GETTEXT_POFILE_DESTINATION)
        set(GETTEXT_POFILE_DESTINATION "${GETTEXT_POTFILE_DESTINATION}")
        message(STATUS "[GETTEXT] POFILE_DESTINATION defaulting to ${GETTEXT_POTFILE_DESTINATION}")
    elseif(NOT GETTEXT_MOFILE_DESTINATION)
        set(GETTEXT_MOFILE_DESTINATION "${GETTEXT_POFILE_DESTINATION}")
        message(STATUS "[GETTEXT] MOFILE_DESTINATION defaulting to ${GETTEXT_POFILE_DESTINATION}")
    elseif(NOT GETTEXT_LANGUAGES)
        message(FATAL_ERROR "[GETTEXT] No LANGUAGES specified")
    elseif(NOT GETTEXT_TARGET_NAME)
        message(FATAL_ERROR "[GETTEXT] No TARGET_NAME specified")
    elseif(NOT GETTEXT_SOURCES)
        message(FATAL_ERROR "[GETTEXT] No SOURCES supplied")
    elseif(AND NOT GETTEXT_INSTALL_DESTINATION)
        set(GETTEXT_INSTALL_DESTINATION "${CMAKE_INSTALL_LOCALEDIR}")
        message(STATUS "[GETTEXT] GETTEXT_INSTALL_DESTINATION defaulting to ${CMAKE_INSTALL_LOCALEDIR}")
    endif()

    # Make input directories absolute in relation to the current directory
    if(NOT IS_ABSOLUTE "${GETTEXT_POTFILE_DESTINATION}")
        set(GETTEXT_POTFILE_DESTINATION "${CMAKE_CURRENT_SOURCE_DIR}/${GETTEXT_POTFILE_DESTINATION}")
        file(TO_CMAKE_PATH "${GETTEXT_POTFILE_DESTINATION}" GETTEXT_POTFILE_DESTINATION)
    endif()
    if(NOT IS_ABSOLUTE "${GETTEXT_POFILE_DESTINATION}")
        set(GETTEXT_POFILE_DESTINATION "${CMAKE_CURRENT_SOURCE_DIR}/${GETTEXT_POFILE_DESTINATION}")
        file(TO_CMAKE_PATH "${GETTEXT_POFILE_DESTINATION}" GETTEXT_POFILE_DESTINATION)
    endif()
    if(NOT IS_ABSOLUTE "${GETTEXT_MOFILE_DESTINATION}")
        set(GETTEXT_MOFILE_DESTINATION "${CMAKE_CURRENT_SOURCE_DIR}/${GETTEXT_MOFILE_DESTINATION}")
        file(TO_CMAKE_PATH "${GETTEXT_MOFILE_DESTINATION}" GETTEXT_MOFILE_DESTINATION)
    endif()

    # Create needed directories
    if(NOT EXISTS "${GETTEXT_POTFILE_DESTINATION}")
        file(MAKE_DIRECTORY "${GETTEXT_POTFILE_DESTINATION}")
    endif()
    if(NOT EXISTS "${GETTEXT_POFILE_DESTINATION}")
        file(MAKE_DIRECTORY "${GETTEXT_POFILE_DESTINATION}")
    endif()
    if(NOT EXISTS "${GETTEXT_MOFILE_DESTINATION}")
        file(MAKE_DIRECTORY "${GETTEXT_MOFILE_DESTINATION}")
    endif()

    if(GETTEXT_ALL)
        add_custom_target("${GETTEXT_TARGET_NAME}" ALL)
    else()
        add_custom_target("${GETTEXT_TARGET_NAME}")
    endif()

    # Generate the .pot file from the program sources
    # sources ---{xgettext}---> .pot
    if(NOT EXISTS "${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot")
        message(STATUS "[GETTEXT] Creating initial ${GETTEXT_DOMAIN}.pot file")
        execute_process(
            COMMAND "${GETTEXT_XGETTEXT_COMMAND}" ${GETTEXT_XGETTEXT_ARGS}
                ${GETTEXT_SOURCES}
                "--output=${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot"
            WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}")
    endif()
    add_custom_command(
        OUTPUT "${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot"
        COMMAND "${GETTEXT_XGETTEXT_COMMAND}" ${GETTEXT_XGETTEXT_ARGS}
            ${GETTEXT_SOURCES}
            "--output=${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot"
        DEPENDS ${GETTEXT_SOURCES}
        WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
        COMMENT "Generating a ${GETTEXT_DOMAIN}.pot file from program sources")

    foreach(lang IN LISTS GETTEXT_LANGUAGES)
        # Create needed directories
        if(NOT EXISTS "${GETTEXT_POFILE_DESTINATION}/${lang}")
            file(MAKE_DIRECTORY "${GETTEXT_POFILE_DESTINATION}/${lang}")
        endif()
        if(NOT EXISTS "${GETTEXT_MOFILE_DESTINATION}/${lang}")
            file(MAKE_DIRECTORY "${GETTEXT_MOFILE_DESTINATION}/${lang}")
        endif()

        # .pot ---{msginit}---> .po
        if(NOT EXISTS "${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po")
            message(STATUS "[GETTEXT] Creating initial ${GETTEXT_DOMAIN}.po file for ${lang}")
            execute_process(
                COMMAND "${GETTEXT_MSGINIT_COMMAND}" ${GETTEXT_MSGINIT_ARGS}
                    "--input=${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot"
                    "--output-file=${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po"
                    "--no-translator"
                    "--locale=${lang}"
                WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}")
        endif()
        add_custom_command(
            OUTPUT "${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po"
            COMMAND "${GETTEXT_MSGMERGE_COMMAND}" ${GETTEXT_MSGMERGE_ARGS}
                "${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po"
                "${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot"
                "--output-file=${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po"
                "--quiet"
            DEPENDS "${GETTEXT_POTFILE_DESTINATION}/${GETTEXT_DOMAIN}.pot"
            WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
            COMMENT "Updating the ${lang} ${GETTEXT_DOMAIN}.po file from the ${GETTEXT_DOMAIN}.pot file")

        add_custom_command(
            OUTPUT "${GETTEXT_MOFILE_DESTINATION}/${lang}/LC_MESSAGES/${GETTEXT_DOMAIN}.mo"
            COMMAND "${GETTEXT_MSGFMT_COMMAND}" ${GETTEXT_MSGFMT_ARGS}
                "${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po"
                "--output-file=${GETTEXT_MOFILE_DESTINATION}/${lang}/LC_MESSAGES/${GETTEXT_DOMAIN}.mo"
            DEPENDS "${GETTEXT_POFILE_DESTINATION}/${lang}/${GETTEXT_DOMAIN}.po"
            WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
            COMMENT "Creating the ${lang} ${GETTEXT_DOMAIN}.mo file from the ${GETTEXT_DOMAIN}.po file")

        add_custom_target("${GETTEXT_TARGET_NAME}_${lang}"
            DEPENDS "${GETTEXT_MOFILE_DESTINATION}/${lang}/LC_MESSAGES/${GETTEXT_DOMAIN}.mo")
        add_dependencies("${GETTEXT_TARGET_NAME}" "${GETTEXT_TARGET_NAME}_${lang}")

        install(FILES "${GETTEXT_MOFILE_DESTINATION}/${lang}/LC_MESSAGES/${GETTEXT_DOMAIN}.mo"
                DESTINATION "${GETTEXT_INSTALL_DESTINATION}/${lang}/LC_MESSAGES/")

    endforeach() # lang IN LISTS GETTEXT_LANGUAGES

endfunction() # add_gettext_domain
